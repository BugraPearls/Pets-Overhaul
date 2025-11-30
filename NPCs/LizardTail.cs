using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PetsOverhaul.Achievements;
using PetsOverhaul.PetEffects;
using PetsOverhaul.Systems;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;

namespace PetsOverhaul.NPCs
{
    public class LizardTail : ModNPC
    {
        public int waitTime = 0;
        public int lifespan = 255;
        public int frameTimer = 0;
        public int nextFrame = 5;
        public int amountOfFrames = 7;
        private int drawArrowTimer = 0;
        private bool switchedArrow = false;
        public override void SetStaticDefaults()
        {
            ContentSamples.NpcBestiaryRarityStars[Type] = 4;
            Main.npcFrameCount[Type] = amountOfFrames;
            NPCID.Sets.NPCBestiaryDrawModifiers drawModifiers = new()
            {
                Velocity = 1f,
            };
            drawModifiers.Position.Y += -10;
            NPCID.Sets.NPCBestiaryDrawOffset[Type] = drawModifiers;
        }
        public override void SetDefaults()
        {
            NPC.aiStyle = -1;
            NPC.width = 18;
            NPC.height = 18;
            NPC.damage = 0;
            NPC.defense = 0;
            NPC.lifeMax = 1;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.friendly = true;
            NPC.dontTakeDamageFromHostiles = false;
            NPC.knockBackResist = 0.6f;
        }
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.AddTags(
                new SpawnConditionBestiaryInfoElement("ItemName.LizardEgg", BestiaryDatabaseNPCsPopulator.CrownosIconIndexes.ItemSpawn, "Images/MapBG14"),
                new FlavorTextBestiaryInfoElement(PetUtils.LocVal("NPCs.LizardTailBestiaryEntry")));
        }
        public override void FindFrame(int frameHeight)
        {
            NPC.frame.Y += frameTimer % nextFrame == 0 ? frameHeight : 0;
            if (frameTimer == 0)
                NPC.frame.Y = 0;

            frameTimer++;
            if (frameTimer >= amountOfFrames * nextFrame)
                frameTimer = 0;
        }
        public override void HitEffect(NPC.HitInfo hit)
        {
            if (NPC.life <= 0)
            {
                for (int i = 0; i < 6; i++)
                {
                    Dust dust = Dust.NewDustDirect(NPC.position, NPC.width, NPC.height, DustID.Lihzahrd, 2 * hit.HitDirection, -2f);
                    if (Main.rand.NextBool(2))
                    {
                        dust.noGravity = true;
                        dust.scale = 1.2f * NPC.scale;
                    }
                    else
                    {
                        dust.scale = 0.7f * NPC.scale;
                    }
                }
                Gore.NewGore(NPC.GetSource_Death(), NPC.Center, Main.rand.NextVector2Circular(2f, 2f), 259, NPC.scale);
            }
        }
        public override void OnSpawn(IEntitySource source)
        {
            if (source is EntitySource_Pet)
            {
                Main.BestiaryTracker.Sights.RegisterWasNearby(NPC);
                NPC.lifeMax = (int)NPC.ai[0];
                waitTime = (int)NPC.ai[1];
                lifespan = (int)NPC.ai[2];
                NPC.life = NPC.lifeMax;
                NPC.netUpdate = true;
            }
        }
        void Kill()
        {
            NPC.life = 0;
            NPC.HitEffect();
            NPC.active = false;
            SoundEngine.PlaySound(SoundID.NPCDeath1, NPC.Center);
        }
        public override Color? GetAlpha(Color drawColor)
        {
            return drawColor with { A = CurrentAlpha() };
        }
        public byte CurrentAlpha()
        {
            int alpha = lifespan / 6 + 1;
            if (alpha > 255)
                alpha = 255;
            return (byte)alpha;
        }
        public override void AI()
        {
            waitTime--;
            lifespan--;

            NPC.velocity *= 0.9f;

            if (waitTime <= 0)
            {
                Player player = Main.player[NPC.FindClosestPlayer()];
                if (NPC.getRect().Intersects(player.getRect()))
                {
                    Lizard lizard = player.GetModPlayer<Lizard>();
                    if (lizard.Player.whoAmI == Main.myPlayer)
                    {
                        Main.BestiaryTracker.Kills.RegisterKill(NPC); //Give Player bestiary Kill count if 'picked up' and complete their achievement
                        ModContent.GetInstance<NutritiousDecoy>().flag.Complete();
                    }
                    lizard.Pet.PetRecovery(player.statLifeMax2, lizard.percentHpRecover, isLifesteal: false);
                    lizard.Pet.timer = (int)(lizard.Pet.timer * lizard.tailCdRefund);

                    Kill();
                    return;
                }
            }
            if (lifespan <= 0)
            {
                Kill();
            }
        }
        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {//ai[3] is only passed as 1 here from LizardTail, which will make the 'arrow' appear.
            if (NPC.ai[3] == 1 && waitTime <= 0 && Main.myPlayer == Main.LocalPlayer.whoAmI)
            {
                if (switchedArrow == false)
                {
                    drawArrowTimer++;
                }
                else
                {
                    drawArrowTimer--;
                }
                if (drawArrowTimer >= 60 || drawArrowTimer <= 0)
                {
                    switchedArrow = !switchedArrow;
                }
                spriteBatch.Draw((Texture2D)Main.Assets.Request<Texture2D>("Images/Item_40"), NPC.position with { Y = NPC.position.Y - 60f + drawArrowTimer * 0.2f } - screenPos, Main.MouseTextColorReal with { A = CurrentAlpha() });
            }
        }
    }
}
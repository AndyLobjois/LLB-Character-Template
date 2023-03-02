using UnityEngine;
using GameplayEntities;
using LLHandlers;

namespace Cactuar
{
    public class CactuarCharacterEntity : PlayerEntity
    {
        private Floatf floatf = new Floatf();
        private Vector2f vector2f = new Vector2f();
        private IBGCBLLKIHA oldDir = new IBGCBLLKIHA();
        private bool ballWasBunted = false;

        public override void Init(object data)
        {
            characterIdentifier = "pampa";
            base.Init(data);
        }

        public override void SetEntityValues()
        {
            base.SetEntityValues();
            base.SetEntityValues();
            gravityForce = floatf.DecimalToFloatf(35m);
            gravityForceUp = floatf.DecimalToFloatf(35m);
            gravityForceApex = floatf.DecimalToFloatf(15m);
            jumpPower = floatf.DecimalToFloatf(10m);
            groundAcc = floatf.DecimalToFloatf(62m);
            groundDeacc = floatf.DecimalToFloatf(62m);
            airAcc = floatf.DecimalToFloatf(50m);
            airDeacc = floatf.DecimalToFloatf(14m);
            maxMove = floatf.DecimalToFloatf(7m);
            maxAirMove = floatf.DecimalToFloatf(7m);
            apexIn = floatf.DecimalToFloatf(3m);
            apexOut = floatf.DecimalToFloatf(-3m);
            gravityForceFastFall = maxGravity;
            extraJumpPower = floatf.DecimalToFloatf(11m);
            extraJumpAmount = 1;
            HHBCPNCDNDH x = floatf.Divide(floatf.One(), floatf.DecimalToFloatf(60.0m));
            smashWindUpDuration = framesDuration60fps(5);
            smashReadyDuration = framesDuration60fps(9);
            smashOverheadSwingDuration = framesDuration60fps(7);
            smashFrontSwingDuration = framesDuration60fps(3);
            smashOutDuration = framesDuration60fps(11);
            hitAngleDown = 7;
            hitAngleUp = 315;
            hitAngleUpForward = hitAngleUp;
            hitAngleNeutralDownAir = 80;
            hitAngleSmash = 40;
            hitAngleDownAirForward = 12;
            hitAngleDownAirBackward = 266;
            specialShineOffset = vector2f.MultiplyByFloatf(vector2f.CreateVector2f(-5, 45), World.FPIXEL_SIZE);
            chargeMaxDuration = floatf.Multiply(x, floatf.DecimalToFloatf(17));
            canJumpCancelCharge = true;
            expressTauntPhase1Duration = floatf.DecimalToFloatf(0.3m);
            expressTauntPhase2Duration = floatf.DecimalToFloatf(1.2m);
            pxHeight = 152;
        }

        public override void SetEntityBoxes()
        {
            HHBCPNCDNDH fpixel_SIZE = World.FPIXEL_SIZE;
            moveBox = AddBox(new Box(GetPosition(), vector2f.Zero(), vector2f.MultiplyByFloatf(vector2f.CreateVector2f(60, pxHeight), fpixel_SIZE), BoxType.MOVEBOX, false));
            moveBox.active = true;
            CreateHurtbox("NORMAL_HURTBOX", vector2f.MultiplyByFloatf(vector2f.CreateVector2f(60, pxHeight), fpixel_SIZE), vector2f.Zero());
            CreateHurtbox("HALF_CROUCH_HURTBOX", vector2f.MultiplyByFloatf(vector2f.CreateVector2f(60, 110), fpixel_SIZE), vector2f.MultiplyByFloatf(vector2f.MultiplyByFloatf(vector2f.Down(), floatf.DecimalToFloatf(21)), fpixel_SIZE));
            CreateHurtbox("CROUCH_HURTBOX", vector2f.MultiplyByFloatf(vector2f.CreateVector2f(80, 74), fpixel_SIZE), vector2f.MultiplyByFloatf(vector2f.MultiplyByFloatf(vector2f.Down(), floatf.DecimalToFloatf(39)), fpixel_SIZE));
            IBGCBLLKIHA sizeSwing = vector2f.CreateVector2f(124, pxHeight);
            IBGCBLLKIHA offsetSwing = vector2f.CreateVector2f(57, 0);
            IBGCBLLKIHA acihfibjnkm = vector2f.CreateVector2f(110, 70);
            IBGCBLLKIHA acihfibjnkm2 = vector2f.CreateVector2f(0, 111);
            SetFrontHitboxesAndParryBoxes(sizeSwing, offsetSwing);
            CreateHitbox("SMASH_TOP_HITBOX", vector2f.MultiplyByFloatf(acihfibjnkm, fpixel_SIZE), vector2f.MultiplyByFloatf(acihfibjnkm2, fpixel_SIZE), "SMASH_OVERHEAD_HIT", true, default(HHBCPNCDNDH), string.Empty);
            CreateHitbox("DOWN_AIR_HITBOX", vector2f.CreateVector2f(floatf.Multiply(floatf.Multiply(acihfibjnkm.GCPKPHMKLBN, fpixel_SIZE), floatf.DecimalToFloatf(1.1m)), HHBCPNCDNDH.AJOCFFLIIIH(floatf.DecimalToFloatf(170), fpixel_SIZE)), vector2f.CreateVector2f(floatf.DecimalToFloatf(0), HHBCPNCDNDH.AJOCFFLIIIH(floatf.DecimalToFloatf(-95), fpixel_SIZE)), "DOWN_AIR_HIT", false, default(HHBCPNCDNDH), string.Empty);
            PlayerHitbox playerHitbox = CreateHitbox("BUNT_HITBOX", vector2f.MultiplyByFloatf(vector2f.CreateVector2f(90, 78), fpixel_SIZE), vector2f.MultiplyByFloatf(vector2f.CreateVector2f(75, 77), fpixel_SIZE), "BUNT_HIT", false, floatf.DecimalToFloatf(0.08m), string.Empty);
            playerHitbox.bunts = true;
            playerHitbox = CreateHitbox("BUNT_HITBOX2", vector2f.MultiplyByFloatf(vector2f.CreateVector2f(126, 114), fpixel_SIZE), vector2f.MultiplyByFloatf(vector2f.CreateVector2f(57, -19), fpixel_SIZE), "BUNT_HIT", false, floatf.DecimalToFloatf(0.08m), string.Empty);
            playerHitbox.bunts = true;
            CreateHurtbox("LIE_HURTBOX", vector2f.MultiplyByFloatf(vector2f.CreateVector2f(sizeSwing.CGJJEHPPOAN, floatf.DecimalToFloatf(45)), fpixel_SIZE), vector2f.MultiplyByFloatf(vector2f.MultiplyByFloatf(vector2f.Down(), floatf.DecimalToFloatf(40)), fpixel_SIZE));
            playerHitbox = CreateHitbox("SHIELD_HITBOX", IBGCBLLKIHA.AJOCFFLIIIH(new IBGCBLLKIHA(175, 175), fpixel_SIZE), IBGCBLLKIHA.AJOCFFLIIIH(new IBGCBLLKIHA(0, 5), fpixel_SIZE), "SHIELD_HIT", false, framesDuration60fps(12), string.Empty);
            playerHitbox.stunState = HitstunState.HIT_BY_SOUNDBLAST_STUN;
        }

        public override void SetEntityAbilities()
        {
            AddAbility(new ShieldAbility(this));
            base.SetEntityAbilities();
        }

        public void Update()
        {
            bool resetColor = false;
            if (GetCurrentAbilityState() != null)
            {
                if (GetCurrentAbilityState().name == "SWING_CHARGE")
                {
                    if (HHBCPNCDNDH.OAHDEOGKOIM(abilityData.abilityStateTimer, framesDuration60fps(3)) && HHBCPNCDNDH.HGDAIHMEFKC(HHBCPNCDNDH.AJOCFFLIIIH(abilityData.abilityStateTimer, HHBCPNCDNDH.NKKIFJJEPOL(20))) % 2 != 0)
                    {
                        foreach (Renderer r in skinRenderers)
                        {
                            if (!r.gameObject.name.EndsWith("Outline"))
                            {
                                r.material.SetColor("_EffectColor", new Color(0.8f, 0.8f, 0.8f, 1));
                            }
                        }
                    }
                    else resetColor = true;
                }
                else resetColor = true;
            }
            if (resetColor)
            {
                foreach (Renderer r in skinRenderers)
                {
                    if (!r.gameObject.name.EndsWith("Outline"))
                    {
                        r.material.SetColor("_EffectColor", new Color(0f, 0f, 0f, 1));
                    }
                }
            }
        }


        public override void UpdateHitPause()
        {
            if (playerData.hitPauseState == HitPauseState.SWING_PAUSE && CanActivateSpecialFromHitstun() && (NewSpecialInput() || playerData.bufferedSpecial))
            {
                for (int i = 0; i < playerData.trackedHitEntityID.Count; i++)
                {
                    if (world.GetEntity(playerData.trackedHitEntityID[i]).IsBall())
                    {
                        if (((BallEntity)world.GetEntity(playerData.trackedHitEntityID[i])).CanActivateSpecialOnThisBall())
                        {
                            ActivateSpecial();
                        }
                    }
                }
            }
            base.UpdateHitPause();
        }


        public override void HandleHitboxHit(string boxName, HitableEntity hitEntity)
        {
            if (boxName == "SHIELD_HITBOX")
            {
                if (hitEntity.IsBall())
                {
                    BallEntity ball = (BallEntity)hitEntity;

                    if (ball.ballData.ballState == BallState.BUNTED) ballWasBunted = true;
                    else ballWasBunted = false;
                    oldDir = hitEntity.hitableData.flyDirection;
                }
            }
            base.HandleHitboxHit(boxName, hitEntity);
        }

        public override IBGCBLLKIHA GetHitDirection(HitstunState victimHitstunState)
        {
            if (playerData.abilityState == "SHIELD_HIT")
            {
                World world = World.instance;
                for (int i = 0; i < playerData.trackedHitEntityID.Count; i++)
                {
                    if (playerData.trackedHitEntityID[i] != 0 && world.GetEntity(playerData.trackedHitEntityID[i]).IsBall())
                    {
                        BallEntity ballEntity = (BallEntity)world.GetEntity(playerData.trackedHitEntityID[i]);
                        if (ballWasBunted)
                        {
                            if (GetFlipHitAngle(victimHitstunState)) return oldDir;
                            else return new IBGCBLLKIHA(HHBCPNCDNDH.AJOCFFLIIIH(oldDir.GCPKPHMKLBN, new HHBCPNCDNDH(-1)), oldDir.CGJJEHPPOAN);
                        }
                        else
                        {
                            if (GetFlipHitAngle(victimHitstunState)) return new IBGCBLLKIHA(oldDir.GCPKPHMKLBN, HHBCPNCDNDH.AJOCFFLIIIH(oldDir.CGJJEHPPOAN, new HHBCPNCDNDH(-1)));
                            else return new IBGCBLLKIHA(HHBCPNCDNDH.AJOCFFLIIIH(oldDir.GCPKPHMKLBN, new HHBCPNCDNDH(-1)), HHBCPNCDNDH.AJOCFFLIIIH(oldDir.CGJJEHPPOAN, new HHBCPNCDNDH(-1)));
                        }
                    }
                }
            }
            return base.GetHitDirection(victimHitstunState);
        }


        private void ActivateSpecial()
        {
            BallEntity ballEntity = null;
            for (int i = 0; i < playerData.trackedHitEntityID.Count; i++)
            {
                if (playerData.trackedHitEntityID[i] != 0)
                {
                    if (world.GetEntity(playerData.trackedHitEntityID[i]).IsBall())
                    {
                        ballEntity = (BallEntity)world.GetEntity(playerData.trackedHitEntityID[i]);
                        Hit(ballEntity, true, floatf.DecimalToFloatf(0.67m), HitstunState.SWING_STUN, true, true);
                        CreateSpecialSpikeEffect(ballEntity, 0.67f);
                        SpecialActivated(ballEntity, true, true, true);
                        EndHitPause();
                    }
                }
            }
            SpecialActivated(ballEntity, true, true, true);
        }

        public override void DoTauntEffects()
        {
            PlayVoice(Voice.SCORE);
            IBGCBLLKIHA ibgcbllkiha = IBGCBLLKIHA.AJOCFFLIIIH(new IBGCBLLKIHA(25, -7), World.FPIXEL_SIZE);
            if (playerData.heading == Side.LEFT)
            {
                ibgcbllkiha.GCPKPHMKLBN = HHBCPNCDNDH.GANELPBAOPN(ibgcbllkiha.GCPKPHMKLBN);
            }
            effectHandler.CreateSpecialReadyEffect(ibgcbllkiha, playerIndex);
            base.DoTauntEffects();
        }

        private void CreateSpecialSpikeEffect(BallEntity ballEntity, float duration)
        {
            EffectEntity spareEffect = EffectHandler.instance.GetSpareEffect();
            EffectData effectData = new EffectData(1);
            effectData.active = true;
            effectData.SetPositionData(ballEntity.GetPosition());
            effectData.frameSizePixels = new JKMAAHELEMF(512, 64);
            effectData.scale = 0.75f;
            effectData.rotation = (float)floatf.ToDecimal(Math.FlyDirectionToAngle(ballEntity.ballData.flyDirection)) - 90;
            effectData.animSpeed = HHBCPNCDNDH.NKKIFJJEPOL(32);
            effectData.loopAnim = true;
            effectData.duration = duration;
            effectData.layer = Layer.GAMEPLAY;
            effectData.graphicName = "special_spike";
            effectData.isSpecialAbilityEffect = true;
            spareEffect.ApplyEffectData(effectData, false);
        }

    }
}

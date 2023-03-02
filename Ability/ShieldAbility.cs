using System.Collections.Generic;
using GameplayEntities;
using LLHandlers;
using Abilities;

namespace Cactuar
{
    public class ShieldAbility : Ability
    {
        private HHBCPNCDNDH stateDuration1;
        private HHBCPNCDNDH stateDuration2;

        public ShieldAbility(AbilityEntity abilityEntity)
        {
            Init("shield", abilityEntity, InputAction.JUMP, AbilityGroundType.BOTH, int.MaxValue, true, PlayerState.ACTION);
            activateOnGroundType = AbilityGroundType.GROUND;
            playerState = PlayerState.ACTION;
            activatedByHand = true;
            stateDuration1 = framesDuration60fps(12);
            stateDuration2 = framesDuration60fps(12);
            AddState(new AbilityState(PlayerState.ACTION, "SHIELD1", "", "special", stateDuration1, new List<string>
            {
                "SHIELD_HITBOX"
            }, AbilityGroundType.NONE)).hurtboxes = new List<string>
            {
                "NORMAL_HURTBOX"
            };
            AddState(new AbilityState(PlayerState.ACTION, "SOUNDBLAST2", "", "specialOut", stateDuration2, null, AbilityGroundType.NONE)).hurtboxes = new List<string>
            {
                "NORMAL_HURTBOX"
            };
            AddSingleState(new AbilityState(PlayerState.HITPAUSE, "SHIELD_HIT", "SOUNDBLAST2", "specialOut", new List<string>
            {
                "SHIELD_HITBOX"
            }, AbilityGroundType.AIR));
        }

        // Token: 0x06000036 RID: 54 RVA: 0x00003D3C File Offset: 0x00001F3C
        public override void SetAbilityState(string state)
        {
            if (state == "SHIELD1")
            {
                CreateShieldEffect();
                GameCamera.StartWave(IBGCBLLKIHA.GAFCIOAEGKD(entity.GetCurrentlyActiveHurtbox().bounds.LOLBPNFNKMI, IBGCBLLKIHA.AJOCFFLIIIH(IBGCBLLKIHA.AJOCFFLIIIH(IBGCBLLKIHA.GGFFJDILCDA, HHBCPNCDNDH.NKKIFJJEPOL(30)), World.FPIXEL_SIZE)), false, default(HHBCPNCDNDH));
                entity.moveableData.velocity = new IBGCBLLKIHA(new HHBCPNCDNDH(0), new HHBCPNCDNDH(0));
                entity.PlaySfx(Sfx.DEFLECT);
                entity.ResetEnergy();
            }

            base.SetAbilityState(state);
        }

        private void CreateShieldEffect()
        {
            EffectEntity spareEffect = EffectHandler.instance.GetSpareEffect();
            EffectData effectData = new EffectData(1);
            effectData.active = true;
            effectData.SetPositionData(entity.GetPosition());
            effectData.frameSizePixels = new JKMAAHELEMF(256, 256);
            effectData.scale = 0.7f;
            effectData.animSpeed = HHBCPNCDNDH.NKKIFJJEPOL(32);
            effectData.endAtAnimEnd = true;
            effectData.layer = Layer.INFRONT_GAMEPLAY;
            effectData.graphicName = "cactus_shield";
            spareEffect.ApplyEffectData(effectData, false);
        }

        public static bool HookedCrouchAbilityUpdate(CrouchAbility crouchAbility)
        {
            AbilityEntity entity = crouchAbility.entity;
            if (entity.HasFullEnergy() && entity.character == (Character)50 && entity.GetInputNew(LLHandlers.InputAction.JUMP))
            {
                entity.StartAbility("shield");
                return true;
            }
            else return false;
        }
    }
}

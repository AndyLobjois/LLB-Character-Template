using System;
using UnityEngine;

namespace Cactuar
{
    public class CactuarCharacterModel : CactuarCharacterEntity
    {
        public override void SetEntityVisuals()
        {
            AOOJOMIECLD modelValues = JPLELOFJOOH.NEBGBODHHCG((Character)50, this.variant);
            standardScale = modelValues.OGMOEKPOBBP;
            try { SetVisualModel("main", modelValues, true, true); } catch (Exception ex) { Debug.Log("CACTUAR: " + ex); }
            throwingHand = GetVisual("main").gameObject.transform.Find("Armature/root 1/sp1/arm1r/arm2r/BallHolder/BallHolder_end");
            AddBaseVisuals();
            headTf = GetVisual("main").gameObject.transform.Find("root/sp1/head");
        }


        public override void SetEntityAnimations()
        {
            AddClipsToAnimInfo();
            SetAnimInfo("jump", HHBCPNCDNDH.NKKIFJJEPOL(1m), false, string.Empty, 0f, false);
            SetAnimInfo("jumpToFall", HHBCPNCDNDH.NKKIFJJEPOL(1m), false, "fallHold", 0f, false);
            SetAnimInfo("jumpForward", HHBCPNCDNDH.NKKIFJJEPOL(1m), false, string.Empty, 0f, false);
            SetAnimInfo("jumpForwardToFall", HHBCPNCDNDH.NKKIFJJEPOL(1m), false, "fallHold", 0f, false);
            SetAnimInfo("land", HHBCPNCDNDH.NKKIFJJEPOL(1.0m), false, "idle", 0f, false);
            SetAnimInfo("abilityLand", HHBCPNCDNDH.NKKIFJJEPOL(1.0m), false, string.Empty, 0f, false);
            SetAnimInfo("getUp", HHBCPNCDNDH.NKKIFJJEPOL(1.0m), false, string.Empty, 0f, false);
            SetAnimInfo("doubleJump", HHBCPNCDNDH.NKKIFJJEPOL(1m), false, string.Empty, 0f, false);
            SetAnimInfo("startRun", HHBCPNCDNDH.NKKIFJJEPOL(1.0m), false, "startRunToRun", 0f, false);
            SetAnimInfo("startRunToRun", HHBCPNCDNDH.NKKIFJJEPOL(1), false, "run", 0f, false);
            SetAnimInfo("stop", HHBCPNCDNDH.NKKIFJJEPOL(1.0m), false, "idle", 0f, false);
            SetAnimInfo("turn", HHBCPNCDNDH.NKKIFJJEPOL(1.0m), false, "idle", 0f, false);
            SetAnimInfo("swingHit", HHBCPNCDNDH.NKKIFJJEPOL(1.0m), false, string.Empty, 0f, false);
            SetAnimInfo("downSwingHit", HHBCPNCDNDH.NKKIFJJEPOL(1.0m), false, string.Empty, 0f, false);
            SetAnimInfo("smashFrontHit", HHBCPNCDNDH.NKKIFJJEPOL(1.0m), false, string.Empty, 0f, false);
            SetAnimInfo("smashOverheadHit", HHBCPNCDNDH.NKKIFJJEPOL(1.0m), false, string.Empty, 0f, false);
            SetAnimInfo("getHit", HHBCPNCDNDH.NKKIFJJEPOL(1), false, string.Empty, 0f, false);
            SetAnimInfo("groundKnockback", HHBCPNCDNDH.NKKIFJJEPOL(1), false, string.Empty, 0f, false);
            SetAnimInfo("wallJump", HHBCPNCDNDH.NKKIFJJEPOL(1), false, "fallHold", 0f, false);
            SetAnimInfo("wallSlideStart", HHBCPNCDNDH.NKKIFJJEPOL(1.0m), false, "wallSlide", 0f, false);
            SetAnimInfo("lieDown", HHBCPNCDNDH.NKKIFJJEPOL(1.0m), false, string.Empty, 0f, false);
            SetAnimInfo("groundKnockback", HHBCPNCDNDH.NKKIFJJEPOL(1.0m), false, "idle", 0f, false);
            SetAnimInfo("dash", HHBCPNCDNDH.NKKIFJJEPOL(1.0m), false, string.Empty, 0f, false);
            SetAnimInfo("dashIn", HHBCPNCDNDH.NKKIFJJEPOL(1.0m), false, string.Empty, 0f, false);
            SetAnimInfo("dashOut", HHBCPNCDNDH.NKKIFJJEPOL(1.0m), false, string.Empty, 0f, false);
            SetAnimInfo("groundParryKnockback", HHBCPNCDNDH.NKKIFJJEPOL(1.0m), false, "idle", 0f, false);
            TurnAllEffectVisualsOff();
        }
    }
}

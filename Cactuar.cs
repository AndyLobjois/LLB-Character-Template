using BepInEx;

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using LLHandlers;
using GameplayEntities;
using LLScreen;
using UnityEngine.Networking;
using System.Collections;
using System.IO;
using Multiplayer;

namespace Cactuar
{
    [BepInPlugin("LLBCharacterTemplate", "CharacterTemplate", "1.0.0.0")]
    public class Cactuar : BaseUnityPlugin {
        private static Cactuar instance = null;
        public static Cactuar Instance { get { return instance; } }
        public static void Initialize() { GameObject gameObject = new GameObject("Cactuar"); Cactuar modLoader = gameObject.AddComponent<Cactuar>(); DontDestroyOnLoad(gameObject); instance = modLoader; }

        private const string modVersion = "v1.0";
        private const string repositoryOwner = "MrGentle";
        private const string repositoryName = "LLBMM-Cactuar";
        private string resourceFolder = Application.dataPath + @"\Managed\CactuarResources";

        ScreenPlayers screenPlayers;
        ScreenUnlocksCharacters screenUnlocksCharacters;
        ScreenUnlocksSkins screenUnlocksSkins;
        GameObject characterButton;

        //Mod Menu stuff
        ModMenuIntegration MMI;
        public bool tournamentMode;


        //Online stuff
        bool[] playerHasCactuar = new bool[4];
        bool shouldShowCactuarButton = false;
        JOFJHDJHJGI gameState;
        int resendPacketTimer = 30;

        bool assignedsHuds = false;
        List<GameHudPlayerInfo> huds = new List<GameHudPlayerInfo>();

        //Character info
        Character customCharacter = (Character)50; //The identifier of the character, try to keep it unique.
        string displayName = "CACTUAR"; //The character name to display in lobbies, and ingame.

        string displayType = "CACTUS";
        string displayAge = "0 to 200";
        string displayBio = "Few know that this reclusive desert-dwelling creature lashes together individual needles for its 1000 Needle attack, a fact which suggests that Cactuars are in fact intelligent enough for tool use.";

        //Variant names
        string variant1 = "Gum by Hang";
        string variant2 = "Grape by Hang";
        string variant3 = "Gladiator by Mocha";
        string variant4 = "Robot by Aru";
        string variant5 = "Black Liquorice by wallace bean";
        string variant6 = "Tron by imt";
        string variant7 = "PJ-Banana by Gentle";
        string variant8 = "Noise Clone";
        string alt_variant1 = "Sheriff by Aru";
        string alt_variant2 = "Cowgirl by Mocha";

        //Assets
        AudioClip announceClip;
        Sprite buttonPortrait;
        Sprite characterIcon;
        Texture2D shieldEffect;
        Texture2D spikeEffect;


        private void Start() {
            //Setup
            CopyBundlesToBundlesFolder(resourceFolder + @"\Bundles\");

            //Model Assets
            AddMeshInfoToGame("pampa", 1f, 76); //(Name of gameobject within bundle, mesh scale, ingame up/down offset);
            AddVariantInfoToGame(CharacterVariant.DEFAULT, "pampa", "pampaMat");
            AddVariantInfoToGame(CharacterVariant.ALT0, "pampa", "pampaMat_Alt00");
            AddVariantInfoToGame(CharacterVariant.ALT1, "pampa", "pampaMat_Alt01");
            AddVariantInfoToGame(CharacterVariant.ALT2, "pampa", "pampaMat_Alt02");
            AddVariantInfoToGame(CharacterVariant.ALT3, "pampa", "pampaMat_Alt03");
            AddVariantInfoToGame(CharacterVariant.ALT4, "pampa", "pampaMat_Alt04");
            AddVariantInfoToGame(CharacterVariant.ALT5, "pampa", "pampaMat_Alt05");
            AddVariantInfoToGame(CharacterVariant.ALT6, "pampa", "pampaMat_Alt06");
            AddVariantInfoToGame(CharacterVariant.STATIC_ALT, "pampa", "ScreenSpaceNoiseOverlayMat");

            AddMeshInfoToGame("pampaAlt", 1f, 76); //(Name of gameobject within bundle, mesh scale, ingame up/down offset);
            AddVariantInfoToGame(CharacterVariant.MODEL_ALT, "pampaAlt", "pampaAltMat");
            AddVariantInfoToGame(CharacterVariant.MODEL_ALT2, "pampaAlt", "pampaAltMat_Alt00");

            //Assets
            StartCoroutine(GetAnnounceClip());
            AddVoiceInfo("pampa_gamewin", "pampa_gethit", "pampa_jump", "pampa_score", "pampa_special_activation", "pampa_intense_swing", "pampa_normal_swing");
            buttonPortrait = LoadButtonPortrait(resourceFolder + @"\Images\" + "pampa_portrait.png");
            characterIcon = LoadButtonPortrait(resourceFolder + @"\Images\" + "pampa_icon.png");
            shieldEffect = LoadEffect(resourceFolder + @"\Images\" + "shield.png");
            spikeEffect = LoadEffect(resourceFolder + @"\Images\" + "specialSpike.png");
        }

        private void FixedUpdate() {

            if (gameState == (JOFJHDJHJGI)5) {
                List<ALDOKEMAOMB> players = ALDOKEMAOMB.LGGEDLHOFIO.ToList();
                List<int> playersToCheck = new List<int>();
                foreach (ALDOKEMAOMB player in players) {
                    var playerID = player.CJFLMDNNMIE;
                    var isLocal = player.GAFCIHKIGNM;
                    var isAI = player.ALBOPCLADGN;
                    var inMatch = player.NGLDMOLLPLK;
                    if (isLocal && inMatch && !isAI) {
                        if (resendPacketTimer == 0) {
                            if (shouldShowCactuarButton == false) {
                                if (player.DOFCCEDJODB == customCharacter || player.LALEEFJMMLH == customCharacter) {
                                    player.DOFCCEDJODB = Character.NONE;
                                    player.LALEEFJMMLH = Character.NONE;
                                }
                            }
                            //Debug.Log("SentPacket");
                            P2P.SendOthers(new Message(Msg.NONE, playerID, (int)customCharacter));
                            resendPacketTimer = 30;
                            playerHasCactuar[playerID] = true;
                        }
                    }
                    if (inMatch && !isAI) {
                        playersToCheck.Add(playerID);
                    }
                }

                if (!tournamentMode) {
                    if (playersToCheck.Count() > 1) {
                        shouldShowCactuarButton = true;
                        foreach (int playerToCheck in playersToCheck) {
                            if (!playerHasCactuar[playerToCheck])
                                shouldShowCactuarButton = false;
                        }
                    } else
                        shouldShowCactuarButton = false;
                }


            } else if (gameState == (JOFJHDJHJGI)11 || gameState == (JOFJHDJHJGI)12 || gameState == (JOFJHDJHJGI)4 || gameState == (JOFJHDJHJGI)6 || gameState == (JOFJHDJHJGI)23) {
                shouldShowCactuarButton = true;
                for (var i = 0; i < playerHasCactuar.Length; i++) {
                    playerHasCactuar[i] = false;
                }
            } else {
                shouldShowCactuarButton = false;
                for (var i = 0; i < playerHasCactuar.Length; i++) {
                    playerHasCactuar[i] = false;
                }
            }

            if (resendPacketTimer > 0)
                resendPacketTimer--;
        }

        private void Update() {
            if (MMI == null) { MMI = gameObject.AddComponent<ModMenuIntegration>(); } else {
                if (InMenu()) {
                    tournamentMode = MMI.GetTrueFalse(MMI.configBools["(bool)tournamentMode"]);
                }
            }

            gameState = DNPFJHMAIBP.HHMOGKIMBNM();

            if (screenPlayers == null)
                screenPlayers = FindObjectOfType<ScreenPlayers>();
            else {
                // Check if we have added the custom character button to the character select screen. if not, add it.
                var addedToCharacterSelection = false;
                foreach (PlayersCharacterButton button in screenPlayers.characterButtons)
                    if (button.gameObject.name == displayName + "ButtonPlayersSelection")
                        addedToCharacterSelection = true;
                if (addedToCharacterSelection == false)
                    AddCustomCharacterButtonToScreenPlayers(screenPlayers);

                if (characterButton != null) {
                    if (shouldShowCactuarButton) {
                        characterButton.SetActive(true);
                    } else {
                        characterButton.SetActive(false);
                    }
                }

                UpdateButtonPositions(screenPlayers);
            }


            if (screenUnlocksCharacters == null)
                screenUnlocksCharacters = FindObjectOfType<ScreenUnlocksCharacters>();
            else {
                // Check if we have added the custom character button to the character showcase screen. if not, add it.
                var addedToCharacterShowcase = false;
                foreach (PlayersCharacterButton button in screenUnlocksCharacters.characterButtons)
                    if (button.gameObject.name == displayName + "ButtonShowcase")
                        addedToCharacterShowcase = true;
                if (addedToCharacterShowcase == false)
                    AddCustomCharacterButtonToScreenCharacterUnlocks(screenUnlocksCharacters);
            }

            GameHudPlayerInfo[] hudsArray = FindObjectsOfType<GameHudPlayerInfo>();
            if (hudsArray.Count() > 0) {
                if (!assignedsHuds) {
                    foreach (GameHudPlayerInfo hud in hudsArray)
                        if (hud.shownPlayer.LALEEFJMMLH == customCharacter)
                            hud.imIcon.sprite = characterIcon;
                    assignedsHuds = true;
                }
            } else
                assignedsHuds = false;

            FixSilhouetteInSkinUnlocksMenu();
            FixSilhouetteInCharacterSelect();
        }

        #region General Methods
        public bool InMenu() {
            if (UIScreen.currentScreens[0] != null) {
                if (UIScreen.currentScreens[0].screenType == ScreenType.MENU) { return true; } else { return false; }
            } else
                return false;
        }

        private void AddCustomCharacterButtonToScreenPlayers(ScreenPlayers screenPlayers) {
            try {
                characterButton = Instantiate(screenPlayers.pfCharacterButton);
                characterButton.name = displayName + "ButtonPlayersSelection";
                Transform transform = characterButton.transform;
                transform.SetParent(screenPlayers.pnCharacterButtons);
                transform.localScale = Vector3.one * 0.836f;
                transform.localRotation = Quaternion.identity;
                PlayersCharacterButton customCharButton = characterButton.GetComponent<PlayersCharacterButton>();
                Array.Resize(ref screenPlayers.characterButtons, screenPlayers.characterButtons.Length + 1);
                screenPlayers.characterButtons[screenPlayers.characterButtons.Length - 1] = customCharButton;

                customCharButton.character = customCharacter;
                customCharButton.imCharacter.sprite = buttonPortrait;

                customCharButton.btCharacter.onClick = delegate (int pNr) {
                    DNPFJHMAIBP.GKBNNFEAJGO(Msg.SEL_CHAR, pNr, (int)customCharacter);
                };
                customCharButton.btCharacter.onHover = delegate (int pNr) {
                    AudioHandler.PlayMenuSfx(Sfx.LOBBY_CHAR_SELECT);
                    DNPFJHMAIBP.GKBNNFEAJGO(Msg.HOVER_CHAR, pNr, (int)customCharacter);
                };

                Debug.Log(displayName + ": Lobby button creation successful");
            } catch (Exception ex) { Debug.Log(displayName + ": Button creation failed"); Debug.Log(ex); }
        }


        private void UpdateButtonPositions(ScreenPlayers screenPlayers) {
            float num = 64f;

            int nrOfActiveButtons = 0;
            foreach (PlayersCharacterButton PCB in screenPlayers.characterButtons) {
                if (PCB.isActiveAndEnabled)
                    nrOfActiveButtons++;
            }

            float num2 = 0f;
            if (nrOfActiveButtons <= 14)
                num2 = -0.5f * num * (float)nrOfActiveButtons;
            else
                num2 = -0.5f * 55 * 15;

            int skip = 0;
            for (int i = 0; i < screenPlayers.characterButtons.Length; i++) {
                if (screenPlayers.characterButtons[i].isActiveAndEnabled) {
                    if (nrOfActiveButtons <= 14)
                        screenPlayers.characterButtons[i].transform.localPosition = new Vector3(num2 + (float)(i - skip) * num, -208f) + Vector3.right * num * 0.5f;
                    else {
                        screenPlayers.characterButtons[i].transform.localScale = Vector3.one * 0.7f;
                        if ((i - skip) <= 14) {
                            screenPlayers.characterButtons[i].transform.localPosition = new Vector3(num2 + (i - skip) * 55, -192f) + Vector3.right * num * 0.5f;
                        } else if ((i - skip) > 14 && (i - skip) <= 29) {
                            screenPlayers.characterButtons[i].transform.localPosition = new Vector3(num2 + ((i - skip) - 15) * 55, -272f) + Vector3.right * num * 0.5f;
                        }
                    }
                } else
                    skip++;
            }
        }

        private void AddCustomCharacterButtonToScreenCharacterUnlocks(ScreenUnlocksCharacters screenUnlocksCharacters) {
            try {
                GameObject gameObject = Instantiate(screenUnlocksCharacters.pfCharacterButton);
                gameObject.name = displayName + "ButtonShowcase";
                Transform transform = gameObject.transform;
                transform.SetParent(screenUnlocksCharacters.pnCharacterButtons);
                transform.localScale = Vector3.one;
                transform.localRotation = Quaternion.identity;
                PlayersCharacterButton customCharButton = gameObject.GetComponent<PlayersCharacterButton>();
                screenUnlocksCharacters.characterButtons.Add(customCharButton);

                customCharButton.Init(customCharacter);

                customCharButton.character = customCharacter;
                customCharButton.imCharacter.sprite = buttonPortrait;

                customCharButton.btCharacter.onClick = delegate (int pNr) {
                    DNPFJHMAIBP.GKBNNFEAJGO(Msg.SEL_CHAR, pNr, (int)customCharacter);
                };
                customCharButton.btCharacter.onHover = delegate (int pNr) {
                    AudioHandler.PlayMenuSfx(Sfx.LOBBY_CHAR_SELECT);
                    DNPFJHMAIBP.GKBNNFEAJGO(Msg.HOVER_CHAR, pNr, (int)customCharacter);
                };

                for (int i = 0; i < screenUnlocksCharacters.characterButtons.Count(); i++) {
                    transform.localPosition = new Vector3(-screenUnlocksCharacters.columnOffset + (float)(i % screenUnlocksCharacters.columns) * screenUnlocksCharacters.columnOffset + (float)(i / screenUnlocksCharacters.columns) * screenUnlocksCharacters.rowXOffset, (float)(i / screenUnlocksCharacters.columns) * screenUnlocksCharacters.rowYOffset);
                }
                Debug.Log(displayName + ": Showcase character button creation successful");
            } catch (Exception ex) { Debug.Log(displayName + ": Button creation failed"); Debug.Log(ex); }
        }

        private void FixSilhouetteInSkinUnlocksMenu() {
            if (screenUnlocksSkins == null)
                screenUnlocksSkins = FindObjectOfType<ScreenUnlocksSkins>();
            else {
                if (screenUnlocksSkins.previewModel.character == customCharacter) {
                    screenUnlocksSkins.previewModel.SetSilhouette(false);
                }
            }
        }

        private void FixSilhouetteInCharacterSelect() {
            if (screenPlayers == null)
                screenPlayers = FindObjectOfType<ScreenPlayers>();
            else {
                foreach (PlayersSelection ps in screenPlayers.playerSelections) {
                    CharacterModel model = ps.characterModel;
                    model.SetSilhouette(false);
                }
            }
        }
        #endregion

        #region Assets
        private void CopyBundlesToBundlesFolder(string fileLocation) {
            string bundlesPath = Directory.GetParent(Application.dataPath) + @"\Bundles\characters\";
            try {
                if (!File.Exists(bundlesPath + customCharacter.ToString())) {
                    File.Copy(fileLocation + customCharacter.ToString(), bundlesPath + customCharacter.ToString());
                } else {
                    File.Delete(bundlesPath + customCharacter.ToString());
                    File.Copy(fileLocation + customCharacter.ToString(), bundlesPath + customCharacter.ToString());
                }
            } catch (Exception ex) { Debug.Log(ex); }

            try {
                if (!File.Exists(bundlesPath + customCharacter.ToString() + "_game")) {
                    File.Copy(fileLocation + customCharacter.ToString() + "_game", bundlesPath + customCharacter.ToString() + "_game");
                } else {
                    File.Delete(bundlesPath + customCharacter.ToString() + "_game");
                    File.Copy(fileLocation + customCharacter.ToString() + "_game", bundlesPath + customCharacter.ToString() + "_game");
                }
            } catch (Exception ex) { Debug.Log(ex); }
        }

        private void AddMeshInfoToGame(string _nameOfGameObjectInBundle, float _meshScale, int _ingameOffset) {
            try {
                JPLELOFJOOH.GHKGDLBCFPK meshInfo = new JPLELOFJOOH.GHKGDLBCFPK(_nameOfGameObjectInBundle, _meshScale, _ingameOffset); //(Name of gameobject within bundle, mesh scale, ingame up/down offset);

                List<JPLELOFJOOH.GHKGDLBCFPK> meshInfos = JPLELOFJOOH.OGAHHGABFPE.ToList();
                meshInfos.Add(meshInfo);
                JPLELOFJOOH.OGAHHGABFPE = meshInfos.ToArray();
                Debug.Log(displayName + ": Successfully added mesh info: " + meshInfos.ToString() + " [" + _nameOfGameObjectInBundle + " ," + _meshScale.ToString() + " ," + _ingameOffset + "]");
            } catch { Debug.Log(displayName + ": Failed adding mesh info"); }
        }

        private void AddVariantInfoToGame(CharacterVariant _variant, string _nameOfGameObjectInBundle, string _materialName) {
            List<JPLELOFJOOH.NCBHPNHFLAJ> variantInfos = JPLELOFJOOH.LKIFMPEFNGB.ToList();
            if (_variant == CharacterVariant.STATIC_ALT && _materialName == "ScreenSpaceNoiseOverlayMat") {
                variantInfos.Add(new JPLELOFJOOH.NCBHPNHFLAJ(customCharacter, _variant, _nameOfGameObjectInBundle, "ScreenSpaceNoiseOverlayMat", FKBHNEMDBMK.DJKFKKODCCM));
            } else {
                variantInfos.Add(new JPLELOFJOOH.NCBHPNHFLAJ(customCharacter, _variant, _nameOfGameObjectInBundle, _materialName, FKBHNEMDBMK.NMJDMHNMDNJ));
            }
            JPLELOFJOOH.LKIFMPEFNGB = variantInfos.ToArray();
        }

        IEnumerator GetAnnounceClip() {
            using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(resourceFolder + @"\Audio\" + "announcer_pampa.wav", AudioType.WAV)) {
                yield return www.SendWebRequest();

                if (www.isDone) {
                    announceClip = DownloadHandlerAudioClip.GetContent(www);
                    Debug.Log(displayName + ": Successfully imported AnnounceClip: [" + announceClip.ToString() + "]");
                }

                if (www.isNetworkError)
                    Debug.Log(displayName + ": Failed importing AnnounceClip");
            }
        }

        private void AddVoiceInfo(string _gamewin, string _gethit, string _jump, string _score, string _special_activation, string _intense_swing, string _normal_swing) {
            AudioHandler.AddVoice(new CharVoice(customCharacter, Voice.GAMEWIN), _gamewin, 60, true, true, false);
            AudioHandler.AddVoice(new CharVoice(customCharacter, Voice.GETHIT), _gethit, 40, true, true, false);
            AudioHandler.AddVoice(new CharVoice(customCharacter, Voice.JUMP), _jump, 10, true, true, false);
            AudioHandler.AddVoice(new CharVoice(customCharacter, Voice.SCORE), _score, 50, true, true, false);
            AudioHandler.AddVoice(new CharVoice(customCharacter, Voice.SPECIAL_ACTIVATION), _special_activation, 30, true, true, false);
            AudioHandler.AddVoice(new CharVoice(customCharacter, Voice.SWING_INTENSE), _intense_swing, 10, true, true, false);
            AudioHandler.AddVoice(new CharVoice(customCharacter, Voice.SWING_NORMAL), _normal_swing, 10, true, true, false);
        }

        public Sprite LoadButtonPortrait(string _path) //Loads a png from a file and returns it (Loads the asset into memory, do only load it once)
        {
            Sprite ret = null;

            if (!File.Exists(_path)) {
                Debug.Log("Could not find " + _path);
            }
            Texture2D tex = null;
            byte[] fileData;

            fileData = File.ReadAllBytes(_path);
            tex = new Texture2D(2, 2);
            tex.LoadImage(fileData);

            ret = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));

            if (ret == null)
                Debug.Log(displayName + ": Failed importing ButtonPortrait");
            else
                Debug.Log(displayName + ": Successfully loaded ButtonPortrait: [" + ret.ToString() + "]");

            return ret;
        }

        public Sprite LoadCharacterIcon(string _path) {
            Sprite ret = null;

            if (!File.Exists(_path)) {
                Debug.Log("Could not find " + _path);
            }
            Texture2D tex = null;
            byte[] fileData;

            fileData = File.ReadAllBytes(_path);
            tex = new Texture2D(2, 2);
            tex.LoadImage(fileData);

            ret = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));

            if (ret == null)
                Debug.Log(displayName + ": Failed importing CharacterIcon");
            else
                Debug.Log(displayName + ": Successfully loaded CharacterIcon: [" + ret.ToString() + "]");

            return ret;
        }

        public Texture2D LoadEffect(string _path) //Loads a png from a file and returns it (Loads the asset into memory, do only load it once)
        {
            if (!File.Exists(_path)) {
                Debug.Log("Could not find " + _path);
            }
            Texture2D tex = null;
            byte[] fileData;

            fileData = File.ReadAllBytes(_path);
            tex = new Texture2D(2, 2);
            tex.LoadImage(fileData);

            if (tex == null)
                Debug.Log(displayName + ": Failed importing " + _path + " effect");
            else
                Debug.Log(displayName + ": Successfully loaded " + _path + " effect: [" + tex.ToString() + "]");

            return tex;
        }

        #endregion

        #region Methods for injection

        public static bool HookedGetRandomCharacter(out Character character, ref Character[] skip) {
            character = instance.customCharacter;
            if (instance.shouldShowCactuarButton && !instance.tournamentMode) {
                if (instance.screenPlayers == null)
                    instance.screenPlayers = FindObjectOfType<ScreenPlayers>();
                else {
                    if (instance.screenPlayers.characterButtons.Length > 0) {
                        List<ALDOKEMAOMB> players = ALDOKEMAOMB.LGGEDLHOFIO.ToList();
                        List<int> playersToCheck = new List<int>();
                        foreach (ALDOKEMAOMB player in players) {
                            var isLocal = player.GAFCIHKIGNM;
                            var isAI = player.ALBOPCLADGN;
                            var inMatch = player.NGLDMOLLPLK;
                            if (isLocal && inMatch && !isAI) {
                                List<global::Character> list = global::EPCDKLCABNC.LMJIMGAAKDI((!player.GAFCIHKIGNM) ? player.CJFLMDNNMIE : -1);

                                int max = (instance.screenPlayers.characterButtons.Length - 1) - ((instance.screenPlayers.characterButtons.Length - 1) - list.Count);

                                int result = UnityEngine.Random.Range(0, list.Count + 1);
                                if (result == 3) {
                                    return true;
                                }
                            }
                        }
                    }
                }
            }
            return false;
        }


        public static void HookedOnReceiveMessage(ref Envelope envelope) {
            try {
                Message message = envelope.message;
                if (message.msg == Msg.NONE) {
                    if (message.index == (int)instance.customCharacter) {
                        //Debug.Log("Recieved Message: " + message.msg.ToString() + " From PlayerNR: " + message.playerNr.ToString());
                        instance.playerHasCactuar[message.playerNr] = true;
                    }
                }
            } catch { }
            //HBHMJHDDJIF
        }

        public static void HookedPlayerLeft(ref int playerNr) {
            //Debug.Log("PlayerLeft: " + playerNr.ToString());
            instance.playerHasCactuar[playerNr] = false;
            //LHNLAJBJKJD
        }

        public static bool HookedCreatePlayerEntity(out PlayerEntity pe, ALDOKEMAOMB player) {
            Character character = player.LALEEFJMMLH;
            pe = null;
            if (character == instance.customCharacter) {
                PlayerHandler ph = World.instance.playerHandler;
                GameObject go = new GameObject();
                pe = go.AddComponent<CactuarCharacterModel>();
                player.JCCIAMJEODH = pe;
                pe.character = instance.customCharacter;
                pe.variant = player.AIINAIDBHJI;
                pe.player = player;
                pe.playerIndex = player.CJFLMDNNMIE;
                ph.playerHandlerData.playerData[player.CJFLMDNNMIE].team = player.HEOKEMBMDIJ;
                pe.Init(ph.playerHandlerData.playerData[player.CJFLMDNNMIE]);
                pe.tf.parent = ph.holder;
                go.name = pe.characterIdentifier + pe.entityName;

                if (player.NGLDMOLLPLK)
                    pe.SetPlayerState(PlayerState.STANDBY, string.Empty, HitPauseState.NONE, HitstunState.NONE);
                else
                    pe.SetPlayerState(PlayerState.DISABLED, string.Empty, HitPauseState.NONE, HitstunState.NONE);

                pe.UpdateUnityTransform();
            }
            if (pe != null) {
                Debug.Log(instance.displayName + ": Successfully created custom PlayerEntity");
                return true;
            } else {
                Debug.Log(instance.displayName + ": Failed creating custom PlayerEntity");
                return false;
            }
        }

        public static bool HookedPlayAnnounce(ref Character character) {
            if (character == instance.customCharacter) {
                AudioHandler.audioSourceAnnounce.PlayOneShot(instance.announceClip);
                return true;
            }
            return false;
        }

        public static bool HookedGetCharacterName(out string str, ref Character character) {
            str = instance.displayName;
            if (character == instance.customCharacter) {
                return true;
            }
            return false;
        }

        public static bool HookedVariantIsUnlocked(out bool ret, Character character, CharacterVariant charVar, int num) {
            if (character == instance.customCharacter) { ret = true; return true; } else { ret = false; return false; }
        }

        public static bool HookedSetCharInfo(ref Character character) {
            if (character == instance.customCharacter) {
                TextHandler.SetText(instance.screenPlayers.lbCharInfo, string.Concat(new string[]
                {
                    "<size=50><color=yellow>",
                    instance.displayName,
                    "</color></size>\n<color=yellow>",
                    TextHandler.Get("CHARINFO_TYPE", new string[0]),
                    ":  </color><color=white>",
                    instance.displayType,
                    "</color>\n<color=yellow>",
                    TextHandler.Get("CHARINFO_AGE", new string[0]),
                    ":  </color><color=white>",
                    instance.displayAge,
                    "</color>\n<color=yellow>",
                    TextHandler.Get("CHARINFO_BIO", new string[0]),
                    ":  </color><color=white>",
                    instance.displayBio,
                    "</color>"
                }));
                return true;
            } else
                return false;
        }

        public static bool HookedSetSkinNames(out string name, ref Character character, ref int nr) {
            name = "???";
            if (character == instance.customCharacter) {
                switch (nr) {
                    case 1:
                    name = instance.variant1;
                    break;
                    case 2:
                    name = instance.variant2;
                    break;
                    case 3:
                    name = instance.variant3;
                    break;
                    case 4:
                    name = instance.variant4;
                    break;
                    case 5:
                    name = instance.variant5;
                    break;
                    case 6:
                    name = instance.variant6;
                    break;
                    case 7:
                    name = instance.variant7;
                    break;
                    case 8:
                    name = instance.variant8;
                    break;
                    case 9:
                    name = instance.alt_variant1;
                    break;
                    case 10:
                    name = instance.alt_variant2;
                    break;
                }
                return true;
            } else
                return false;
        }

        public static bool HookedLoadEffectTexture(out Texture2D tex, ref string textureName, ref bool b) {
            tex = null;
            if (textureName == "cactus_shield") {
                tex = instance.shieldEffect;
                return true;
            } else if (textureName == "special_spike") {
                tex = instance.spikeEffect;
                return true;
            } else
                return false;
        }

        #endregion
    }

    //public class Cactuar : MonoBehaviour
    //{
    //    private static Cactuar instance = null;
    //    public static Cactuar Instance { get { return instance; } }
    //    public static void Initialize() { GameObject gameObject = new GameObject("Cactuar"); Cactuar modLoader = gameObject.AddComponent<Cactuar>(); DontDestroyOnLoad(gameObject); instance = modLoader; }

    //    private const string modVersion = "v1.0";
    //    private const string repositoryOwner = "MrGentle";
    //    private const string repositoryName = "LLBMM-Cactuar";
    //    private string resourceFolder = Application.dataPath + @"\Managed\CactuarResources";

    //    ScreenPlayers screenPlayers;
    //    ScreenUnlocksCharacters screenUnlocksCharacters;
    //    ScreenUnlocksSkins screenUnlocksSkins;
    //    GameObject characterButton;

    //    //Mod Menu stuff
    //    //ModMenuIntegration MMI;
    //    public bool tournamentMode;


    //    //Online stuff
    //    bool[] playerHasCactuar = new bool[4];
    //    bool shouldShowCactuarButton = false;
    //    JOFJHDJHJGI gameState;
    //    int resendPacketTimer = 30;

    //    bool assignedsHuds = false;
    //    List<GameHudPlayerInfo> huds = new List<GameHudPlayerInfo>();

    //    //Character info
    //    Character customCharacter = (Character)50; //The identifier of the character, try to keep it unique.
    //    string displayName = "CACTUAR"; //The character name to display in lobbies, and ingame.

    //    string displayType = "CACTUS";
    //    string displayAge = "0 to 200";
    //    string displayBio = "Few know that this reclusive desert-dwelling creature lashes together individual needles for its 1000 Needle attack, a fact which suggests that Cactuars are in fact intelligent enough for tool use.";

    //    //Variant names
    //    string variant1 = "Gum by Hang";
    //    string variant2 = "Grape by Hang";
    //    string variant3 = "Gladiator by Mocha";
    //    string variant4 = "Robot by Aru";
    //    string variant5 = "Black Liquorice by wallace bean";
    //    string variant6 = "Tron by imt";
    //    string variant7 = "PJ-Banana by Gentle";
    //    string variant8 = "Noise Clone";
    //    string alt_variant1 = "Sheriff by Aru";
    //    string alt_variant2 = "Cowgirl by Mocha";

    //    //Assets
    //    AudioClip announceClip;
    //    Sprite buttonPortrait;
    //    Sprite characterIcon;
    //    Texture2D shieldEffect;
    //    Texture2D spikeEffect;


    //    private void Start()
    //    {
    //        //Setup
    //        CopyBundlesToBundlesFolder(resourceFolder + @"\Bundles\");

    //        //Model Assets
    //        AddMeshInfoToGame("pampa", 1f, 76); //(Name of gameobject within bundle, mesh scale, ingame up/down offset);
    //        AddVariantInfoToGame(CharacterVariant.DEFAULT, "pampa", "pampaMat");
    //        AddVariantInfoToGame(CharacterVariant.ALT0, "pampa", "pampaMat_Alt00");
    //        AddVariantInfoToGame(CharacterVariant.ALT1, "pampa", "pampaMat_Alt01");
    //        AddVariantInfoToGame(CharacterVariant.ALT2, "pampa", "pampaMat_Alt02");
    //        AddVariantInfoToGame(CharacterVariant.ALT3, "pampa", "pampaMat_Alt03");
    //        AddVariantInfoToGame(CharacterVariant.ALT4, "pampa", "pampaMat_Alt04");
    //        AddVariantInfoToGame(CharacterVariant.ALT5, "pampa", "pampaMat_Alt05");
    //        AddVariantInfoToGame(CharacterVariant.ALT6, "pampa", "pampaMat_Alt06");
    //        AddVariantInfoToGame(CharacterVariant.STATIC_ALT, "pampa", "ScreenSpaceNoiseOverlayMat");

    //        AddMeshInfoToGame("pampaAlt", 1f, 76); //(Name of gameobject within bundle, mesh scale, ingame up/down offset);
    //        AddVariantInfoToGame(CharacterVariant.MODEL_ALT, "pampaAlt", "pampaAltMat");
    //        AddVariantInfoToGame(CharacterVariant.MODEL_ALT2, "pampaAlt", "pampaAltMat_Alt00");

    //        //Assets
    //        StartCoroutine(GetAnnounceClip());
    //        AddVoiceInfo("pampa_gamewin", "pampa_gethit", "pampa_jump", "pampa_score", "pampa_special_activation", "pampa_intense_swing", "pampa_normal_swing");
    //        buttonPortrait = LoadButtonPortrait(resourceFolder + @"\Images\" + "pampa_portrait.png");
    //        characterIcon = LoadButtonPortrait(resourceFolder + @"\Images\" + "pampa_icon.png");
    //        shieldEffect = LoadEffect(resourceFolder + @"\Images\" + "shield.png");
    //        spikeEffect = LoadEffect(resourceFolder + @"\Images\" + "specialSpike.png");
    //    }

    //    private void FixedUpdate()
    //    {

    //        if (gameState == (JOFJHDJHJGI)5)
    //        {
    //            List<ALDOKEMAOMB> players = ALDOKEMAOMB.LGGEDLHOFIO.ToList();
    //            List<int> playersToCheck = new List<int>();
    //            foreach (ALDOKEMAOMB player in players)
    //            {
    //                var playerID = player.CJFLMDNNMIE;
    //                var isLocal = player.GAFCIHKIGNM;
    //                var isAI = player.ALBOPCLADGN;
    //                var inMatch = player.NGLDMOLLPLK;
    //                if (isLocal && inMatch && !isAI)
    //                {
    //                    if (resendPacketTimer == 0)
    //                    {
    //                        if (shouldShowCactuarButton == false)
    //                        {
    //                            if (player.DOFCCEDJODB == customCharacter || player.LALEEFJMMLH == customCharacter)
    //                            {
    //                                player.DOFCCEDJODB = Character.NONE;
    //                                player.LALEEFJMMLH = Character.NONE;
    //                            }
    //                        }
    //                        //Debug.Log("SentPacket");
    //                        P2P.SendOthers(new Message(Msg.NONE, playerID, (int)customCharacter));
    //                        resendPacketTimer = 30;
    //                        playerHasCactuar[playerID] = true;
    //                    }
    //                }
    //                if (inMatch && !isAI)
    //                {
    //                    playersToCheck.Add(playerID);
    //                }
    //            }

    //            if (!tournamentMode)
    //            {
    //                if (playersToCheck.Count() > 1)
    //                {
    //                    shouldShowCactuarButton = true;
    //                    foreach (int playerToCheck in playersToCheck)
    //                    {
    //                        if (!playerHasCactuar[playerToCheck]) shouldShowCactuarButton = false;
    //                    }
    //                }
    //                else shouldShowCactuarButton = false;
    //            }


    //        }
    //        else if (gameState == (JOFJHDJHJGI)11 || gameState == (JOFJHDJHJGI)12 || gameState == (JOFJHDJHJGI)4 || gameState == (JOFJHDJHJGI)6 || gameState == (JOFJHDJHJGI)23)
    //        {
    //            shouldShowCactuarButton = true;
    //            for (var i = 0; i < playerHasCactuar.Length; i++)
    //            {
    //                playerHasCactuar[i] = false;
    //            }
    //        }
    //        else
    //        {
    //            shouldShowCactuarButton = false;
    //            for (var i = 0; i < playerHasCactuar.Length; i++)
    //            {
    //                playerHasCactuar[i] = false;
    //            }
    //        }

    //        if (resendPacketTimer > 0) resendPacketTimer--;
    //    }

    //    private void Update()
    //    {
    //        //if (MMI == null) { MMI = gameObject.AddComponent<ModMenuIntegration>(); }
    //        //else
    //        //{
    //        //    if (InMenu()) {
    //        //        tournamentMode = MMI.GetTrueFalse(MMI.configBools["(bool)tournamentMode"]);
    //        //    }
    //        //}

    //        gameState = DNPFJHMAIBP.HHMOGKIMBNM();

    //        if (screenPlayers == null) screenPlayers = FindObjectOfType<ScreenPlayers>();
    //        else
    //        {
    //            // Check if we have added the custom character button to the character select screen. if not, add it.
    //            var addedToCharacterSelection = false;
    //            foreach (PlayersCharacterButton button in screenPlayers.characterButtons) if (button.gameObject.name == displayName + "ButtonPlayersSelection") addedToCharacterSelection = true;
    //            if (addedToCharacterSelection == false) AddCustomCharacterButtonToScreenPlayers(screenPlayers);

    //            if (characterButton != null)
    //            {
    //                if (shouldShowCactuarButton)
    //                {
    //                    characterButton.SetActive(true);
    //                }
    //                else
    //                {
    //                    characterButton.SetActive(false);
    //                }
    //            }

    //            UpdateButtonPositions(screenPlayers);
    //        }


    //        if (screenUnlocksCharacters == null) screenUnlocksCharacters = FindObjectOfType<ScreenUnlocksCharacters>();
    //        else
    //        {
    //            // Check if we have added the custom character button to the character showcase screen. if not, add it.
    //            var addedToCharacterShowcase = false;
    //            foreach (PlayersCharacterButton button in screenUnlocksCharacters.characterButtons) if (button.gameObject.name == displayName + "ButtonShowcase") addedToCharacterShowcase = true;
    //            if (addedToCharacterShowcase == false) AddCustomCharacterButtonToScreenCharacterUnlocks(screenUnlocksCharacters);
    //        }

    //        GameHudPlayerInfo[] hudsArray = FindObjectsOfType<GameHudPlayerInfo>();
    //        if (hudsArray.Count() > 0)
    //        {
    //            if (!assignedsHuds)
    //            {
    //                foreach (GameHudPlayerInfo hud in hudsArray) if (hud.shownPlayer.LALEEFJMMLH == customCharacter) hud.imIcon.sprite = characterIcon;
    //                assignedsHuds = true;
    //            }
    //        }
    //        else assignedsHuds = false;

    //        FixSilhouetteInSkinUnlocksMenu();
    //        FixSilhouetteInCharacterSelect();
    //    }

    //    #region General Methods
    //    public bool InMenu()
    //    {
    //        if (UIScreen.currentScreens[0] != null)
    //        {
    //            if (UIScreen.currentScreens[0].screenType == ScreenType.MENU)
    //            { return true; }
    //            else { return false; }
    //        }
    //        else return false;
    //    }

    //    private void AddCustomCharacterButtonToScreenPlayers(ScreenPlayers screenPlayers)
    //    {
    //        try
    //        {
    //            characterButton = Instantiate(screenPlayers.pfCharacterButton);
    //            characterButton.name = displayName + "ButtonPlayersSelection";
    //            Transform transform = characterButton.transform;
    //            transform.SetParent(screenPlayers.pnCharacterButtons);
    //            transform.localScale = Vector3.one * 0.836f;
    //            transform.localRotation = Quaternion.identity;
    //            PlayersCharacterButton customCharButton = characterButton.GetComponent<PlayersCharacterButton>();
    //            Array.Resize(ref screenPlayers.characterButtons, screenPlayers.characterButtons.Length + 1);
    //            screenPlayers.characterButtons[screenPlayers.characterButtons.Length - 1] = customCharButton;

    //            customCharButton.character = customCharacter;
    //            customCharButton.imCharacter.sprite = buttonPortrait;

    //            customCharButton.btCharacter.onClick = delegate (int pNr)
    //            {
    //                DNPFJHMAIBP.GKBNNFEAJGO(Msg.SEL_CHAR, pNr, (int)customCharacter);
    //            };
    //            customCharButton.btCharacter.onHover = delegate (int pNr)
    //            {
    //                AudioHandler.PlayMenuSfx(Sfx.LOBBY_CHAR_SELECT);
    //                DNPFJHMAIBP.GKBNNFEAJGO(Msg.HOVER_CHAR, pNr, (int)customCharacter);
    //            };

    //            Debug.Log(displayName + ": Lobby button creation successful");
    //        }
    //        catch (Exception ex) { Debug.Log(displayName + ": Button creation failed"); Debug.Log(ex); }
    //    }


    //    private void UpdateButtonPositions(ScreenPlayers screenPlayers)
    //    {
    //        float num = 64f;

    //        int nrOfActiveButtons = 0;
    //        foreach (PlayersCharacterButton PCB in screenPlayers.characterButtons)
    //        {
    //            if (PCB.isActiveAndEnabled) nrOfActiveButtons++;
    //        }

    //        float num2 = 0f;
    //        if (nrOfActiveButtons <= 14) num2 = -0.5f * num * (float)nrOfActiveButtons;
    //        else num2 = -0.5f * 55 * 15;

    //        int skip = 0;
    //        for (int i = 0; i < screenPlayers.characterButtons.Length; i++)
    //        {
    //            if (screenPlayers.characterButtons[i].isActiveAndEnabled)
    //            {
    //                if (nrOfActiveButtons <= 14) screenPlayers.characterButtons[i].transform.localPosition = new Vector3(num2 + (float)(i - skip) * num, -208f) + Vector3.right * num * 0.5f;
    //                else
    //                {
    //                    screenPlayers.characterButtons[i].transform.localScale = Vector3.one * 0.7f;
    //                    if ((i - skip) <= 14)
    //                    {
    //                        screenPlayers.characterButtons[i].transform.localPosition = new Vector3(num2 + (i - skip) * 55, -192f) + Vector3.right * num * 0.5f;
    //                    }
    //                    else if ((i - skip) > 14 && (i - skip) <= 29)
    //                    {
    //                        screenPlayers.characterButtons[i].transform.localPosition = new Vector3(num2 + ((i - skip) - 15) * 55, -272f) + Vector3.right * num * 0.5f;
    //                    }
    //                }
    //            }
    //            else skip++;
    //        }
    //    }

    //    private void AddCustomCharacterButtonToScreenCharacterUnlocks(ScreenUnlocksCharacters screenUnlocksCharacters)
    //    {
    //        try
    //        {
    //            GameObject gameObject = Instantiate(screenUnlocksCharacters.pfCharacterButton);
    //            gameObject.name = displayName + "ButtonShowcase";
    //            Transform transform = gameObject.transform;
    //            transform.SetParent(screenUnlocksCharacters.pnCharacterButtons);
    //            transform.localScale = Vector3.one;
    //            transform.localRotation = Quaternion.identity;
    //            PlayersCharacterButton customCharButton = gameObject.GetComponent<PlayersCharacterButton>();
    //            screenUnlocksCharacters.characterButtons.Add(customCharButton);

    //            customCharButton.Init(customCharacter);

    //            customCharButton.character = customCharacter;
    //            customCharButton.imCharacter.sprite = buttonPortrait;

    //            customCharButton.btCharacter.onClick = delegate (int pNr)
    //            {
    //                DNPFJHMAIBP.GKBNNFEAJGO(Msg.SEL_CHAR, pNr, (int)customCharacter);
    //            };
    //            customCharButton.btCharacter.onHover = delegate (int pNr)
    //            {
    //                AudioHandler.PlayMenuSfx(Sfx.LOBBY_CHAR_SELECT);
    //                DNPFJHMAIBP.GKBNNFEAJGO(Msg.HOVER_CHAR, pNr, (int)customCharacter);
    //            };

    //            for (int i = 0; i < screenUnlocksCharacters.characterButtons.Count(); i++)
    //            {
    //                transform.localPosition = new Vector3(-screenUnlocksCharacters.columnOffset + (float)(i % screenUnlocksCharacters.columns) * screenUnlocksCharacters.columnOffset + (float)(i / screenUnlocksCharacters.columns) * screenUnlocksCharacters.rowXOffset, (float)(i / screenUnlocksCharacters.columns) * screenUnlocksCharacters.rowYOffset);
    //            }
    //            Debug.Log(displayName + ": Showcase character button creation successful");
    //        }
    //        catch (Exception ex) { Debug.Log(displayName + ": Button creation failed"); Debug.Log(ex); }
    //    }

    //    private void FixSilhouetteInSkinUnlocksMenu()
    //    {
    //        if (screenUnlocksSkins == null) screenUnlocksSkins = FindObjectOfType<ScreenUnlocksSkins>();
    //        else
    //        {
    //            if (screenUnlocksSkins.previewModel.character == customCharacter)
    //            {
    //                screenUnlocksSkins.previewModel.SetSilhouette(false);
    //            }
    //        }
    //    }

    //    private void FixSilhouetteInCharacterSelect()
    //    {
    //        if (screenPlayers == null) screenPlayers = FindObjectOfType<ScreenPlayers>();
    //        else
    //        {
    //            foreach (PlayersSelection ps in screenPlayers.playerSelections)
    //            {
    //                CharacterModel model = ps.characterModel;
    //                model.SetSilhouette(false);
    //            }
    //        }
    //    }
    //    #endregion

    //    #region Assets
    //    private void CopyBundlesToBundlesFolder(string fileLocation)
    //    {
    //        string bundlesPath = Directory.GetParent(Application.dataPath) + @"\Bundles\characters\";
    //        try
    //        {
    //            if (!File.Exists(bundlesPath + customCharacter.ToString()))
    //            {
    //                File.Copy(fileLocation + customCharacter.ToString(), bundlesPath + customCharacter.ToString());
    //            }
    //            else
    //            {
    //                File.Delete(bundlesPath + customCharacter.ToString());
    //                File.Copy(fileLocation + customCharacter.ToString(), bundlesPath + customCharacter.ToString());
    //            }
    //        }
    //        catch (Exception ex) { Debug.Log(ex); }

    //        try
    //        {
    //            if (!File.Exists(bundlesPath + customCharacter.ToString() + "_game"))
    //            {
    //                File.Copy(fileLocation + customCharacter.ToString() + "_game", bundlesPath + customCharacter.ToString() + "_game");
    //            }
    //            else
    //            {
    //                File.Delete(bundlesPath + customCharacter.ToString() + "_game");
    //                File.Copy(fileLocation + customCharacter.ToString() + "_game", bundlesPath + customCharacter.ToString() + "_game");
    //            }
    //        }
    //        catch (Exception ex) { Debug.Log(ex); }
    //    }

    //    private void AddMeshInfoToGame(string _nameOfGameObjectInBundle, float _meshScale, int _ingameOffset)
    //    {
    //        try
    //        {
    //            JPLELOFJOOH.GHKGDLBCFPK meshInfo = new JPLELOFJOOH.GHKGDLBCFPK(_nameOfGameObjectInBundle, _meshScale, _ingameOffset); //(Name of gameobject within bundle, mesh scale, ingame up/down offset);

    //            List<JPLELOFJOOH.GHKGDLBCFPK> meshInfos = JPLELOFJOOH.OGAHHGABFPE.ToList();
    //            meshInfos.Add(meshInfo);
    //            JPLELOFJOOH.OGAHHGABFPE = meshInfos.ToArray();
    //            Debug.Log(displayName + ": Successfully added mesh info: " + meshInfos.ToString() + " [" + _nameOfGameObjectInBundle + " ," + _meshScale.ToString() + " ," + _ingameOffset + "]");
    //        }
    //        catch { Debug.Log(displayName + ": Failed adding mesh info"); }
    //    }

    //    private void AddVariantInfoToGame(CharacterVariant _variant, string _nameOfGameObjectInBundle, string _materialName)
    //    {
    //        List<JPLELOFJOOH.NCBHPNHFLAJ> variantInfos = JPLELOFJOOH.LKIFMPEFNGB.ToList();
    //        if (_variant == CharacterVariant.STATIC_ALT && _materialName == "ScreenSpaceNoiseOverlayMat")
    //        {
    //            variantInfos.Add(new JPLELOFJOOH.NCBHPNHFLAJ(customCharacter, _variant, _nameOfGameObjectInBundle, "ScreenSpaceNoiseOverlayMat", FKBHNEMDBMK.DJKFKKODCCM));
    //        }
    //        else
    //        {
    //            variantInfos.Add(new JPLELOFJOOH.NCBHPNHFLAJ(customCharacter, _variant, _nameOfGameObjectInBundle, _materialName, FKBHNEMDBMK.NMJDMHNMDNJ));
    //        }
    //        JPLELOFJOOH.LKIFMPEFNGB = variantInfos.ToArray();
    //    }

    //    IEnumerator GetAnnounceClip()
    //    {
    //        using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(resourceFolder + @"\Audio\" + "announcer_pampa.wav", AudioType.WAV))
    //        {
    //            yield return www.SendWebRequest();

    //            if (www.isDone)
    //            {
    //                announceClip = DownloadHandlerAudioClip.GetContent(www);
    //                Debug.Log(displayName + ": Successfully imported AnnounceClip: [" + announceClip.ToString() + "]");
    //            }

    //            if (www.isNetworkError) Debug.Log( displayName + ": Failed importing AnnounceClip");
    //        }
    //    }

    //    private void AddVoiceInfo(string _gamewin, string _gethit, string _jump, string _score, string _special_activation, string _intense_swing, string _normal_swing)
    //    {
    //        AudioHandler.AddVoice(new CharVoice(customCharacter, Voice.GAMEWIN), _gamewin, 60, true, true, false);
    //        AudioHandler.AddVoice(new CharVoice(customCharacter, Voice.GETHIT), _gethit, 40, true, true, false);
    //        AudioHandler.AddVoice(new CharVoice(customCharacter, Voice.JUMP), _jump, 10, true, true, false);
    //        AudioHandler.AddVoice(new CharVoice(customCharacter, Voice.SCORE), _score, 50, true, true, false);
    //        AudioHandler.AddVoice(new CharVoice(customCharacter, Voice.SPECIAL_ACTIVATION), _special_activation, 30, true, true, false);
    //        AudioHandler.AddVoice(new CharVoice(customCharacter, Voice.SWING_INTENSE), _intense_swing, 10, true, true, false);
    //        AudioHandler.AddVoice(new CharVoice(customCharacter, Voice.SWING_NORMAL), _normal_swing, 10, true, true, false);
    //    }

    //    public Sprite LoadButtonPortrait(string _path) //Loads a png from a file and returns it (Loads the asset into memory, do only load it once)
    //    {
    //        Sprite ret = null;

    //        if (!File.Exists(_path))
    //        {
    //            Debug.Log("Could not find " + _path);
    //        }
    //        Texture2D tex = null;
    //        byte[] fileData;

    //        fileData = File.ReadAllBytes(_path);
    //        tex = new Texture2D(2, 2);
    //        tex.LoadImage(fileData);

    //        ret = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));

    //        if (ret == null) Debug.Log(displayName + ": Failed importing ButtonPortrait");
    //        else Debug.Log(displayName + ": Successfully loaded ButtonPortrait: [" + ret.ToString() + "]");

    //        return ret;
    //    }

    //    public Sprite LoadCharacterIcon(string _path)
    //    {
    //        Sprite ret = null;

    //        if (!File.Exists(_path))
    //        {
    //            Debug.Log("Could not find " + _path);
    //        }
    //        Texture2D tex = null;
    //        byte[] fileData;

    //        fileData = File.ReadAllBytes(_path);
    //        tex = new Texture2D(2, 2);
    //        tex.LoadImage(fileData);

    //        ret = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));

    //        if (ret == null) Debug.Log(displayName + ": Failed importing CharacterIcon");
    //        else Debug.Log(displayName + ": Successfully loaded CharacterIcon: [" + ret.ToString() + "]");

    //        return ret;
    //    }

    //    public Texture2D LoadEffect(string _path) //Loads a png from a file and returns it (Loads the asset into memory, do only load it once)
    //    {
    //        if (!File.Exists(_path))
    //        {
    //            Debug.Log("Could not find " + _path);
    //        }
    //        Texture2D tex = null;
    //        byte[] fileData;

    //        fileData = File.ReadAllBytes(_path);
    //        tex = new Texture2D(2, 2);
    //        tex.LoadImage(fileData);

    //        if (tex == null) Debug.Log(displayName + ": Failed importing " + _path + " effect");
    //        else Debug.Log(displayName + ": Successfully loaded " + _path + " effect: [" + tex.ToString() + "]");

    //        return tex;
    //    }

    //    #endregion

    //    #region Methods for injection

    //    public static bool HookedGetRandomCharacter(out Character character, ref Character[] skip)
    //    {
    //        character = instance.customCharacter;
    //        if (instance.shouldShowCactuarButton && !instance.tournamentMode)
    //        {
    //            if (instance.screenPlayers == null) instance.screenPlayers = FindObjectOfType<ScreenPlayers>();
    //            else
    //            {
    //                if (instance.screenPlayers.characterButtons.Length > 0)
    //                {
    //                    List<ALDOKEMAOMB> players = ALDOKEMAOMB.LGGEDLHOFIO.ToList();
    //                    List<int> playersToCheck = new List<int>();
    //                    foreach (ALDOKEMAOMB player in players)
    //                    {
    //                        var isLocal = player.GAFCIHKIGNM;
    //                        var isAI = player.ALBOPCLADGN;
    //                        var inMatch = player.NGLDMOLLPLK;
    //                        if (isLocal && inMatch && !isAI)
    //                        {
    //                            List<global::Character> list = global::EPCDKLCABNC.LMJIMGAAKDI((!player.GAFCIHKIGNM) ? player.CJFLMDNNMIE : -1);

    //                            int max = (instance.screenPlayers.characterButtons.Length - 1) - ((instance.screenPlayers.characterButtons.Length - 1) - list.Count);

    //                            int result = UnityEngine.Random.Range(0, list.Count + 1);
    //                            if (result == 3)
    //                            {
    //                                return true;
    //                            }
    //                        }
    //                    }
    //                }
    //            }
    //        }
    //        return false;
    //    }


    //    public static void HookedOnReceiveMessage(ref Envelope envelope)
    //    {
    //        try
    //        {
    //            Message message = envelope.message;
    //            if (message.msg == Msg.NONE)
    //            {
    //                if (message.index == (int)instance.customCharacter)
    //                {
    //                    //Debug.Log("Recieved Message: " + message.msg.ToString() + " From PlayerNR: " + message.playerNr.ToString());
    //                    instance.playerHasCactuar[message.playerNr] = true;
    //                }
    //            }
    //        }
    //        catch { }
    //        //HBHMJHDDJIF
    //    }

    //    public static void HookedPlayerLeft(ref int playerNr)
    //    {
    //        //Debug.Log("PlayerLeft: " + playerNr.ToString());
    //        instance.playerHasCactuar[playerNr] = false;
    //        //LHNLAJBJKJD
    //    }

    //    public static bool HookedCreatePlayerEntity(out PlayerEntity pe, ALDOKEMAOMB player)
    //    {
    //        Character character = player.LALEEFJMMLH;
    //        pe = null;
    //        if (character == instance.customCharacter)
    //        {
    //            PlayerHandler ph = World.instance.playerHandler;
    //            GameObject go = new GameObject();
    //            pe = go.AddComponent<CactuarCharacterModel>();
    //            player.JCCIAMJEODH = pe;
    //            pe.character = instance.customCharacter;
    //            pe.variant = player.AIINAIDBHJI;
    //            pe.player = player;
    //            pe.playerIndex = player.CJFLMDNNMIE;
    //            ph.playerHandlerData.playerData[player.CJFLMDNNMIE].team = player.HEOKEMBMDIJ;
    //            pe.Init(ph.playerHandlerData.playerData[player.CJFLMDNNMIE]);
    //            pe.tf.parent = ph.holder;
    //            go.name = pe.characterIdentifier + pe.entityName;

    //            if (player.NGLDMOLLPLK) pe.SetPlayerState(PlayerState.STANDBY, string.Empty, HitPauseState.NONE, HitstunState.NONE);
    //            else pe.SetPlayerState(PlayerState.DISABLED, string.Empty, HitPauseState.NONE, HitstunState.NONE);

    //            pe.UpdateUnityTransform();
    //        }
    //        if (pe != null)
    //        {
    //            Debug.Log(instance.displayName + ": Successfully created custom PlayerEntity");
    //            return true;
    //        }
    //        else
    //        {
    //            Debug.Log(instance.displayName + ": Failed creating custom PlayerEntity");
    //            return false;
    //        }
    //    }

    //    public static bool HookedPlayAnnounce(ref Character character)
    //    {
    //        if (character == instance.customCharacter)
    //        {
    //            AudioHandler.audioSourceAnnounce.PlayOneShot(instance.announceClip);
    //            return true;
    //        }
    //        return false;
    //    }

    //    public static bool HookedGetCharacterName(out string str, ref Character character)
    //    {
    //        str = instance.displayName;
    //        if (character == instance.customCharacter)
    //        {
    //            return true;
    //        }
    //        return false;
    //    }

    //    public static bool HookedVariantIsUnlocked(out bool ret, Character character, CharacterVariant charVar, int num)
    //    {
    //        if (character == instance.customCharacter) { ret = true; return true; }
    //        else { ret = false; return false; }
    //    }

    //    public static bool HookedSetCharInfo(ref Character character)
    //    {
    //        if (character == instance.customCharacter)
    //        {
    //            TextHandler.SetText(instance.screenPlayers.lbCharInfo, string.Concat(new string[]
    //            {
    //                "<size=50><color=yellow>",
    //                instance.displayName,
    //                "</color></size>\n<color=yellow>",
    //                TextHandler.Get("CHARINFO_TYPE", new string[0]),
    //                ":  </color><color=white>",
    //                instance.displayType,
    //                "</color>\n<color=yellow>",
    //                TextHandler.Get("CHARINFO_AGE", new string[0]),
    //                ":  </color><color=white>",
    //                instance.displayAge,
    //                "</color>\n<color=yellow>",
    //                TextHandler.Get("CHARINFO_BIO", new string[0]),
    //                ":  </color><color=white>",
    //                instance.displayBio,
    //                "</color>"
    //            }));
    //            return true;
    //        }
    //        else return false;
    //    }

    //    public static bool HookedSetSkinNames(out string name, ref Character character, ref int nr)
    //    {
    //        name = "???";
    //        if (character == instance.customCharacter)
    //        {
    //            switch (nr)
    //            {
    //                case 1: name = instance.variant1; break;
    //                case 2: name = instance.variant2; break;
    //                case 3: name = instance.variant3; break;
    //                case 4: name = instance.variant4; break;
    //                case 5: name = instance.variant5; break;
    //                case 6: name = instance.variant6; break;
    //                case 7: name = instance.variant7; break;
    //                case 8: name = instance.variant8; break;
    //                case 9: name = instance.alt_variant1; break;
    //                case 10: name = instance.alt_variant2; break;
    //            }
    //            return true;
    //        }
    //        else return false;
    //    }

    //    public static bool HookedLoadEffectTexture(out Texture2D tex, ref string textureName, ref bool b)
    //    {
    //        tex = null;
    //        if (textureName == "cactus_shield")
    //        {
    //            tex = instance.shieldEffect;
    //            return true;
    //        }
    //        else if (textureName == "special_spike")
    //        {
    //            tex = instance.spikeEffect;
    //            return true;
    //        }
    //        else return false;
    //    }

    //    #endregion
    //}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SettingsManager : MonoBehaviour
{
    public static SettingsManager instance;
    private State musicState = State.Off;
    private State soundState = State.Off;
    private Lang lang = Lang.English;
    public Lang Lang { get => lang; }
    private event Action<State> _onChangedMusic;
    public event Action<State> OnChangedMusic
    {
        add
        {
            _onChangedMusic+=value;
            _onChangedMusic?.Invoke(musicState);
        }

        remove
        {
            _onChangedMusic -= value;
        }
    }
    private event Action<State> _onChangedSound;
    public event Action<State> OnChangedSound
    {
        add
        {
            _onChangedSound += value;
            _onChangedSound?.Invoke(soundState);
        }

        remove
        {
            _onChangedSound -= value;
        }
    }
    private event Action<Lang> _onChangedLang;
    public event Action<Lang> OnChangedLang
    {
        add
        {
            _onChangedLang += value;
            _onChangedLang?.Invoke(lang);
        }

        remove
        {
            _onChangedLang -= value;
        }
    }
    public const string MUSIC = "music";
    public const string SOUND = "sound";
    public const string LANG = "lang";
    AudioSource audioSource;

    private void Awake()
    {
        instance = this;
        audioSource = GetComponent<AudioSource>();
    }

   private void Start()
    {
        if (GameManager.instance.xmlSettingsDataManager.ExistsData())
        {
            LoadFromFile();
        }
        else
        {
            LoadDefaultSettings();
        }
    }

    public void ChangeMusic(State state)
    {
        musicState = state;
        _onChangedMusic?.Invoke(state);
        if (MySqlManager.IsLogged())
        {
            SaveInDB();
        }
        else
        {
            SaveInFile();
        }
        if(state == State.On)
        {
            audioSource.Play();
        }
        else
        {
            audioSource.Stop();
        }
    }

    public void ChangeSound(State state)
    {
        soundState = state;
        _onChangedSound?.Invoke(state);
        if (MySqlManager.IsLogged())
        {
            SaveInDB();
        }
        else
        {
            SaveInFile();
        }
    }

    public void ChangeLang(Lang lang)
    {
        this.lang = lang;
        _onChangedLang?.Invoke(lang);
        if (MySqlManager.IsLogged())
        {
            SaveInDB();
        }
        else
        {
            SaveInFile();
        }
    }

    public void OnLangChanged(int num)
    {
        if(num == 0)
        {
            ChangeLang(Lang.English);
        }
        else
        {
            ChangeLang(Lang.Russia);
        }
    }

    public void SaveInFile()
    {
        SettingsData settingsData = new SettingsData()
        {
            music = musicState,
            sound=soundState,
            lang = lang
        };
        GameManager.instance.xmlSettingsDataManager.SaveData(settingsData);
    }

    public void LoadFromFile()
    {
        SettingsData settingsData = GameManager.instance.xmlSettingsDataManager.LoadData();
        ChangeLang(settingsData.lang);
        ChangeMusic(settingsData.music);
        ChangeSound(settingsData.sound);
    }

    public void LoadDefaultSettings()
    {
        SettingsData settingsData = GameManager.instance.xmlDefaultSettingsDataManager.LoadData();
        ChangeLang(settingsData.lang);
        ChangeMusic(settingsData.music);
        ChangeSound(settingsData.sound);
    }

    public void SaveInDB()
    {
        if (MySqlManager.ExistsSettingData(MUSIC))
        {
            MySqlManager.ChangeSettingData(MUSIC, musicState.ToString());
        }
        else
        {
            MySqlManager.InsertSettingData(MUSIC, musicState.ToString());
        }
        if (MySqlManager.ExistsSettingData(SOUND))
        {
            MySqlManager.ChangeSettingData(SOUND, soundState.ToString());
        }
        else
        {
            MySqlManager.InsertSettingData(SOUND, soundState.ToString());
        }
        if (MySqlManager.ExistsSettingData(LANG))
        {
            MySqlManager.ChangeSettingData(LANG, lang.ToString());
        }
        else
        {
            MySqlManager.InsertSettingData(LANG, lang.ToString());
        }
    }
    public void LoadFromDB()
    {
        State music = (State)Enum.Parse(typeof(State), MySqlManager.GetSettingData(MUSIC));
        State sound = (State)Enum.Parse(typeof(State), MySqlManager.GetSettingData(SOUND));
        Lang lang = (Lang)Enum.Parse(typeof(Lang), MySqlManager.GetSettingData(LANG));
        ChangeLang(lang);
        ChangeMusic(music);
        ChangeSound(sound);
    }
}

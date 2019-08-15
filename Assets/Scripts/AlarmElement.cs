﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Notifications.iOS;

public class AlarmElement : MonoBehaviour
{
    public GameObject EditAlarmPrefab;

    public Image Profile;
    public Sprite[] profile_sprites;
    public Text TimeString;
    public Button Edit_button;
    public Button X_button;

    // var
    public int profile_idx;
    public int hr_dp_val;
    public int min_dp_val;
    public int ampm_dp_val;
    public int repeat_dp_val;
    public string label_if_val;
    public bool snooze_tg_val;

    public int Id;

    // Start is called before the first frame update
    void Start()
    {
        //AdjustToScreenRatio();
        Edit_button.onClick.AddListener(Edit_button_click);
        X_button.onClick.AddListener(X_button_click);
    }

    public void Edit_button_click()
    {
        var EditUI = Instantiate(EditAlarmPrefab, GameObject.Find("Swipable_right").transform);
        var comp = EditUI.GetComponent<EditAlarm>();
        comp.EditingAlarm = this;
        comp.Id = Id;
        comp.Hour_dropdown.value = hr_dp_val;
        comp.Minute_dropdown.value = min_dp_val;
        comp.AMPM_dropdown.value = ampm_dp_val;
        comp.Repeat_dropdown.value = repeat_dp_val;
        comp.Label_input.text = label_if_val;
        comp.SnoozeToggle.isOn = snooze_tg_val;
        AppManager.Prefabs[0].gameObject.SetActive(false);
    }

    public void X_button_click()
    {
        // remove the ui element
        AlarmList.ui_alarmlist.Remove(gameObject);
        // reorder the ui element list
        AppManager.Prefabs[0].GetComponent<AlarmList>().Reorder();
        // remove the notification
        Notifications.RemoveAlarm(Id);
        AlarmList.SaveAlarmListToFile();
        // destroy this ui element
        Destroy(gameObject);
    }

    const float outline_ratio = 1840f / 365f; // ratio of the image
    public void AdjustToScreenRatio()
    {
        var rt = GetComponent<RectTransform>();
        var newSzie = new Vector2(AppManager.AdjustedWidth, AppManager.AdjustedWidth / outline_ratio);
        var oldSize = rt.sizeDelta;
        var ratio = newSzie.x / oldSize.x;
        rt.sizeDelta = newSzie;

        var profile_rt = Profile.GetComponent<RectTransform>();
        profile_rt.anchoredPosition *= ratio;
        profile_rt.sizeDelta *= ratio;

        TimeString.fontSize = (int)((float)TimeString.fontSize * ratio);
        var timestr_rt = TimeString.GetComponent<RectTransform>();
        timestr_rt.anchoredPosition *= ratio;
        timestr_rt.sizeDelta *= ratio;

        var edit_rt = Edit_button.GetComponent<RectTransform>();
        edit_rt.anchoredPosition *= ratio;
        edit_rt.sizeDelta *= ratio;
        var edit_text = Edit_button.GetComponentInChildren<Text>();
        edit_text.fontSize = (int)((float)edit_text.fontSize * ratio);

        var x_rt = X_button.GetComponent<RectTransform>();
        x_rt.anchoredPosition *= ratio;
        x_rt.sizeDelta *= ratio;
        var x_text = X_button.GetComponentInChildren<Text>();
        x_text.fontSize = (int)((float)x_text.fontSize * ratio);
    }
}
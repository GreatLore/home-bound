﻿using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;

public class SaveManager : MonoBehaviour
{
    public string _patientID;
    private PatientUser _patient;

    private void Awake()
    {
        _patient = GameObject.FindObjectOfType<PatientUser>();
        Debug.Log("Awake - " + _patient);
        //Save();
    }

    public void CreateNewSave()
    {
        GameObject profileForm = GameObject.Find("New Profile Form");

        // get data from form
        InputField lastName = profileForm.transform.Find("LastName/Input").gameObject.GetComponent<InputField>();
        InputField firstName = profileForm.transform.Find("FirstName/Input").gameObject.GetComponent<InputField>();
        InputField age = profileForm.transform.Find("Age/Input").gameObject.GetComponent<InputField>();

        // Generate Patient ID
        string patientID = DateTime.Now.ToString("yyyyMMdd_HHmmssfff");// + lastName.text.GetHashCode();


        PatientInfo patientInfo = new PatientInfo();

        patientInfo.lastName = lastName.text;
        patientInfo.firstName = firstName.text;
        patientInfo.age = age.text;
        patientInfo.patientID = patientID;

        _patient = gameObject.AddComponent(typeof(PatientUser)) as PatientUser;
        _patient.myInfo = patientInfo;

        Save();
        //Debug.Log(patientID);
    }

    public PatientInfo SaveEditedProfile()
    {
        GameObject profileForm = GameObject.Find("Edit Profile Form");

        // get data from form
        InputField lastName = profileForm.transform.Find("LastName/Input").gameObject.GetComponent<InputField>();
        InputField firstName = profileForm.transform.Find("FirstName/Input").gameObject.GetComponent<InputField>();
        InputField age = profileForm.transform.Find("Age/Input").gameObject.GetComponent<InputField>();

        string patientID = GameObject.Find("PatientID/Text").GetComponent<Text>().text;

        PatientInfo patientInfo = new PatientInfo();

        patientInfo.lastName = lastName.text;
        patientInfo.firstName = firstName.text;
        patientInfo.age = age.text;
        patientInfo.patientID = patientID;

        _patient = gameObject.AddComponent(typeof(PatientUser)) as PatientUser;
        _patient.myInfo = patientInfo;

        Save();

        return _patient.myInfo;
    }

    public void Save() {

        // Create/Open the file where we save to
        string folder = Path.Combine(Application.persistentDataPath, "saves");
        if (!Directory.Exists(folder)) {
            Directory.CreateDirectory(folder);
        }

        folder = Path.Combine(folder, _patient.myInfo.patientID);

        if (!Directory.Exists(folder)) {
            Directory.CreateDirectory(folder);
        }

        FileStream file = new FileStream(folder + "/save.dat", FileMode.OpenOrCreate);

        try
        {
            // Binary formatter - allows us to write data to the file
            BinaryFormatter formatter = new BinaryFormatter();
            // Serialization method to write to write data to the file
            formatter.Serialize(file, _patient.myInfo);
        }
        catch (SerializationException e)
        {
            Debug.LogError("Issue with serializing data: " + e.Message);
        }
        finally {
            file.Close();
        }
        
        Debug.Log("Save done - " + _patient.myInfo.patientID);
    }

    public void Load() {
        string folder = Path.Combine(Application.persistentDataPath, "saves");
        folder = Path.Combine(folder, _patientID);

        FileStream file = new FileStream(folder + "/save.dat", FileMode.Open);
        try
        {
            BinaryFormatter formatter = new BinaryFormatter();
            _patient.myInfo = formatter.Deserialize(file) as PatientInfo;
        }
        catch (SerializationException e)
        {
            Debug.LogError("Issue with deserializing data: " + e.Message);
        }
        finally
        {
            file.Close();
        }

        Debug.Log("Load done - " + _patient.myInfo.patientID);
    }

    public PatientInfo LoadInfo(string patientID) {
        string folder = Path.Combine(Application.persistentDataPath, "saves");
        folder = Path.Combine(folder, patientID);

        FileStream file = new FileStream(folder + "/save.dat", FileMode.Open);
        try
        {
            BinaryFormatter formatter = new BinaryFormatter();
            _patient.myInfo = formatter.Deserialize(file) as PatientInfo;
        }
        catch (SerializationException e)
        {
            Debug.LogError("Issue with deserializing data: " + e.Message);
        }
        finally
        {
            file.Close();
        }

        Debug.Log("Load done - " + _patient.myInfo.patientID);

        return _patient.myInfo;
    }

    //function for returning pictures in folder
}

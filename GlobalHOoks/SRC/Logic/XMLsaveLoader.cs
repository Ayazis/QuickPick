using QuickPick.Logic;
using QuickPick.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace QuickPick.SRC.Logic
{
    //class XMLsaveLoader : ISaveLoader
    //{
    //    private string xmlSettings;

    //    public Models.QuickPick QP { get; set; }

    //    public void LoadSettingsFile()
    //    {
    
    //    }

    //    public void LoadSettingsFile(string filePath)
    //    {
       
    //    }

    //    public void SaveSettingsToDisk()
    //    {
    //        //try
    //        //{
    //        //    if (!Directory.Exists(Path.GetDirectoryName(QP.QuickPickModel.SettingsPath)))
    //        //        Directory.CreateDirectory(Path.GetDirectoryName(QP.QuickPickModel.SettingsPath));

                       

    //        //    string fileName = "";
    //        //    var saveDialog = new System.Windows.Forms.SaveFileDialog
    //        //    {
    //        //        InitialDirectory = QP.QuickPickModel.SettingsPath,
    //        //        Filter = "XML files(*.xml)|*.xml"
    //        //    };

    //        //    if (saveDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
    //        //    {
    //        //        fileName = saveDialog.FileName;
    //        //        QP.QuickPickModel.SettingsPath = fileName;

    //        //        // put current model into new settingsClass
    //        //        var settings = new QuickPickSettings(QP.QuickPickModel);
    //        //        var xs = new XmlSerializer(typeof(QuickPickSettings));
    //        //        TextWriter writer = new StreamWriter(Path.GetDirectoryName(fileName));
    //        //        xs.Serialize(writer, settings);
    //        //    }



    //        //    string finalPath = fileName != "" ? fileName : QP.QuickPickModel.SettingsPath;
    //        //    File.WriteAllText(finalPath, xmlSettings);
    //        //}
    //        //catch (Exception ex)
    //        //{
    //        //    Logs.Logger.Log(ex);
    //        //}

    //    }
    //}
}

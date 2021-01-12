using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class SceneReader
{
    private SceneController sc;
    private Actions actions;

    public SceneReader(SceneController sc)
    {
        this.sc = sc;
        actions = sc.Actions;
    }

    public void ReadLines(Scene s)
    {
        if (s.Index >= s.Lines.Count) return;

        var line = s.GetCurrentLine();
        var text = "";

        if (line.Contains("#"))
        {
            while (true)
            {
                if (!line.Contains("#")) break;

                line = line.Replace("#", "");
                
                if (line.Contains("bgm"))
                {
                    line = line.Replace("bgm=", "");
                    sc.SetBGM(line);
                }
                else if (line.Contains("speaker"))
                {
                    line = line.Replace("speaker=", "");
                    sc.SetSpeaker(line);
                }
                else if (line.Contains("addchara"))
                {
                    line = line.Replace("addchara=", "");
                    line = line.Replace("{", "").Replace("}", "");
                    var splitted = line.Split(',');
                    sc.AddCharacter(splitted[0],splitted[1]);
                }
                else if (line.Contains("lumpchara"))
                {
                    //sc.RmCharacter("all");
                    var charas = new List<(string, string)>();
                    while (true)
                    {
                        s.GoNextLine();
                        line = line = s.Lines[s.Index];
                        if (line.Contains("{"))
                        {
                            line = line.Replace("{", "").Replace("}", "");
                            var splitted = line.Split(',');
                            charas.Add((splitted[0], splitted[1]));
                        }
                        else
                        {
                            sc.SetLumpChara(charas);
                            break;
                        }
                    }
                }
                else if (line.Contains("replace"))
                {
                    line = line.Replace("replace_", "");
                    var splitted = line.Split('=');
                    var splitted2 = splitted[1].Split(',');
                    sc.ReplaceChara(splitted[0],splitted2[0],splitted2[1]);
                }
                else if (line.Contains("rmchara"))
                {
                    line = line.Replace("rmchara=", "");
                    sc.RmCharacter(line);
                }
                else if (line.Contains("background"))
                {
                    line = line.Replace("background=","");
                    sc.SetBackground(line);
                }
                else if (line.Contains("simage"))
                {
                    line = line.Replace("simage=", "");
                    var splitted = line.Split(',');
                    sc.SetSImage(splitted[0], splitted[1]);
                }
                else if (line.Contains("image"))
                {
                    line = line.Replace("image_", "");
                    var splitted = line.Split('=');
                    sc.SetImage(splitted[0], splitted[1]);
                }
                else if (line.Contains("stopshake"))
                {
                    sc.StopShake();
                }
                else if (line.Contains("shake"))
                {
                    line = line.Replace("shake=","");
                    var splitted = line.Split(',');
                    sc.Shake(splitted[0], splitted[1]);
                }
                else if (line.Contains("release"))
                {
                    line = line.Replace("release=","");
                    sc.CharaRelease(line);
                }
                else if (line.Contains("nextchapter"))
                {
                    line = line.Replace("nextchapter=", "");
                    sc.NextChapter(line);
                }
                else if (line.Contains("next"))
                {
                    line = line.Replace("next=", "");
                    sc.SetScene(line);
                }
                else if (line.Contains("method"))
                {
                    line = line.Replace("method=", "");
                    var type = actions.GetType();
                    MethodInfo mi = type.GetMethod(line);
                    mi.Invoke(actions, new object[] { });
                }
                else if (line.Contains("options"))
                {
                    var options = new List<(string, string)>();
                    while (true)
                    {
                        s.GoNextLine();
                        line = line = s.Lines[s.Index];
                        if (line.Contains("{"))
                        {
                            line = line.Replace("{", "").Replace("}", "");
                            var splitted = line.Split(',');
                            options.Add((splitted[0], splitted[1]));
                        }
                        else
                        {
                            sc.SetOptions(options);
                            break;
                        }
                    }
                }
                else if (line == "darkon")
                {
                    sc.DarkOn();
                    s.GoNextLine();
                    return;
                }
                else if (line == "darkoff")
                {
                    //if (sc.DarkSeqPlaying()) continue;
                    sc.DarkOff();
                    s.GoNextLine();
                    return;
                }
                else if (line == "stopfallparticle")
                {
                    sc.StopFallParticle();
                }
                else if (line.Contains("fallparticle"))
                {
                    line = line.Replace("fallparticle=","");
                    sc.SetFallParticle(line);
                }
                

                s.GoNextLine();
                if (s.IsFinished()) break;
                line = s.GetCurrentLine();
            }
        }
        if (line.Contains('{'))
        {
            line = line.Replace("{", "");
            
            while (true)
            {
                if (line.Contains('}'))
                {
                    line = line.Replace("}", "");
                    text += line;
                    s.GoNextLine();
                    break;
                }
                else
                {
                    text += line;
                }
                s.GoNextLine();
                if (s.IsFinished()) break;
                line = s.GetCurrentLine();
            }
            sc.SetText(text);
            //if (!string.IsNullOrEmpty(text)) sc.SetText(text);
        }
    }
    
}

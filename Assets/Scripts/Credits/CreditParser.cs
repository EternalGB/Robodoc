using UnityEngine;
using System.Collections.Generic;
using System.Xml;

public class CreditParser
{

	public static List<CreditText> ParseCreditsFile(TextAsset file, GUISkin skin)
	{
		List<CreditText> credits = new List<CreditText>();
		using(XmlReader reader = XmlReader.Create(new System.IO.StringReader(file.text))) {
			while(reader.Read()) {
				switch(reader.NodeType) 
				{
				case XmlNodeType.Element:
					if(reader.Name == "single")
						credits.Add(ParseSingle(reader,skin));
					else if(reader.Name == "double")
						credits.Add(ParseDouble(reader,skin));
					else if(reader.Name == "empty")
						credits.Add(new EmptySpace());
					break;
				default:
					break;
				}
			}
		}
		return credits;
	}

	static CreditText ParseSingle(XmlReader reader, GUISkin skin)
	{
		SingleColumn text = new SingleColumn();
		text.style = skin.GetStyle(reader.GetAttribute("style"));
		text.text = reader.ReadInnerXml();
		//Debug.Log ("Parsed single, Text = " + text.text + ", Style = " + text.style.ToString());
		return text;
	}

	static CreditText ParseDouble(XmlReader reader, GUISkin skin)
	{
		DoubleColumn text = new DoubleColumn();
		reader.ReadToFollowing("dheading");
		text.style = skin.GetStyle(reader.GetAttribute("style"));
		text.text1 = reader.ReadInnerXml();
		reader.ReadToFollowing("dtext");
		text.style2 = skin.GetStyle(reader.GetAttribute("style"));
		text.text2 = reader.ReadInnerXml();
		//Debug.Log ("Parsed Double, Text1 = " + text.text1 + ", Style1 " + text.style.ToString() + ", Text2 = " + text.text2 + ", Style2 = " + text.style2);
		return text;

	}

}


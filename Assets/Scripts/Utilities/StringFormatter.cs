using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/*
 * Sources:
 * - https://www.w3schools.com/html/html_formatting.asp
 * - https://www.w3schools.com/colors/colors_picker.asp
 * - https://www.w3schools.com/html/html_colors.asp
 * - https://www.dofactory.com/html/charset/symbols
 * - https://www.w3schools.com/html/html_formatting.asp
 */

// Formats a string with HTML code.
public class StringFormatter : MonoBehaviour
{
    // TODO: just keep a version of the string saved.

    // If set to 'true', the case is ignored when formatting text.
    public bool ignoreCase = false;

    // TODO: add a function for converting RGBint and RGBfloat format to hex code.
    // The color code (put in hex format).
    [Header("Color")]
    public string colorCode = "";

    // Get set to 'true' if color should be added.
    public bool applyColor = true;

    [Header("Other")]

    // TODO: add more format options.

    // Bold the string.
    public bool bold;

    // Italicize the string.
    public bool italicize;


    // Formats the time string.
    public static string FormatTime(float timeSeconds, bool includeHours = true, bool includeMinutes = true)
    {
        // Multiples for converting times to seconds.
        const float hourToSec = 3600.0F;
        const float minToSec = 60.0F;

        // The hours, minutes, and seconds.
        float hours = 0, minutes = 0, seconds = 0;

        // Hours, minutes, and seconds - floors value so that the remainder can be used.
        // Hours (1 hour = 3600 seconds)
        if(includeHours)
            hours = Mathf.Floor(timeSeconds / hourToSec);

        // Minutes (1 minute = 60 seconds).
        if(includeMinutes)
            minutes = Mathf.Floor((timeSeconds - (hours * hourToSec)) / minToSec);

        // Seconds (round up to remove nanoseconds).
        seconds = Mathf.Ceil(timeSeconds - (minutes * minToSec) - (hours * hourToSec));



        // The time string.
        string timeString = "";

        // Add hours.
        if (includeHours)
            timeString += hours.ToString("00") + ":";

        // Add minutes.
        if (includeMinutes)
            timeString += minutes.ToString("00") + ":";


        // Add seconds.
        timeString += seconds.ToString("00");

        // Returns ther esults.
        return timeString;
    }


    // Formats the string using the string format setting.
    public string FormatString(string str, string substr)
    {
        // If the string or the substring are empty, return an empty string.
        if (str == string.Empty || substr == string.Empty)
            return string.Empty;

        // The element starts and ends.
        Stack<string> starts = new Stack<string>();
        Stack<string> ends = new Stack<string>();

        // Color
        if(applyColor && colorCode != string.Empty)
        {
            // TODO: check for valid color code.
            starts.Push("<color=" + colorCode + ">");
            ends.Push("</color>");
        }

        // Bold
        if(bold)
        {
            starts.Push("<b>");
            ends.Push("</b>");
        }

        // Italicize
        if(italicize)
        {
            starts.Push("<i>");
            ends.Push("</i>");
        }

        // The formatted substring.
        string substrFormatted = substr;
        string formatLeft = "", formatRight = "";

        // While there are elements in the stacks.
        while (starts.Count > 0 && ends.Count > 0)
        {
            // Grabs the start and end of the text format.
            string start = starts.Pop();
            string end = ends.Pop();

            // Add the text element.
            substrFormatted = start + substrFormatted + end;

            // Save the left side of the format, and the right side.
            formatLeft += start;
            formatRight += end;
        }

        // Replace the old substring with its formatted version.
        string newStr = string.Empty;
        
        // Checks if the case should be accoutned for.
        if(ignoreCase)
        {
            // Copies the string and substrings in lower-case form.
            string strLower = str.ToLower();
            string substrLower = substr.ToLower();
            
            // The formatted string.
            string strFormatted = str;

            // The start index of the searches.
            int startIndex = 0;

            // While there are still elements of the substring in the string...
            // And the start index is not at the end of the string.
            while(strLower.IndexOf(substrLower, startIndex) != -1 && startIndex < strLower.Length)
            {
                // Finds the index of the substring.
                startIndex = strLower.IndexOf(substrLower, startIndex);

                // Removes the substring from the lower-version.
                strLower = strFormatted.Remove(startIndex, substrLower.Length);

                // Saves the original substring, and removes it from the formatted string.
                string origSubstr = strFormatted.Substring(startIndex, substr.Length);
                strFormatted = strFormatted.Remove(startIndex, substr.Length);

                // Inserts the formatted substring into the string lower and the string formatted.
                strLower = strLower.Insert(startIndex, substrFormatted.ToLower());

                // Inserts the original substring with the formatting sections attached.
                strFormatted = strFormatted.Insert(startIndex, 
                    formatLeft +
                    origSubstr + 
                    formatRight);

                // Increase the start index by the length of the substring formatted.
                startIndex += substrFormatted.Length;
            }

            // Set the new string to the string forwmatted.
            newStr = strFormatted;
        }    
        else
        {
            // Replace all occurances of the substring with the formatted substring.
            newStr = str.Replace(substr, substrFormatted);
        }

        // Returns the new string.
        return newStr;
    }

    // NOTE: it's best just to keep the original string unformatted and store it.

    //// Returns the string with no formatting.
    //// NOTE: this hard codes the symbols to remove, which may not be ideal.
    //public static string GetStringNoFormatting(string str)
    //{
    //    // OLD
    //    // // The indexes of the left bracket and right bracket.
    //    // int leftIndex = str.IndexOf("<");
    //    // int rightIndex = str.IndexOf(">");
    //    // 
    //    // // While <> can still be found in the string.
    //    // while (leftIndex != -1 && rightIndex != -1)
    //    // {
    //    //     // TODO: this was being buggy for accounting for brackets outside of formats "(< <>)", so I took it out.
    //    //     // Fix this later.
    //    // 
    //    //     // // Checks to make sure the left index is in the right place.
    //    //     // // Only do this if the left index is less than the right index.
    //    //     // if(leftIndex < rightIndex)
    //    //     // {
    //    //     //     // Tries to find the last iteration if "<" in the segment.
    //    //     //     int leftIndex2 = 0;
    //    //     // 
    //    //     //     // Searches the string segment.
    //    //     //     Debug.Log("STRLEN: " + str.Length.ToString() + "| LI: " + leftIndex + " | Count: " + Mathf.Abs(rightIndex - leftIndex).ToString());
    //    //     //     leftIndex2 = str.LastIndexOf(str, leftIndex, Mathf.Abs(rightIndex - leftIndex));
    //    //     // 
    //    //     //     // If a greater index has been found, set that as the left index.
    //    //     //     if (leftIndex2 > leftIndex)
    //    //     //         leftIndex = leftIndex2;
    //    //     // }
    //    // 
    //    //     // Swaps the indexes if the left comes after the right.
    //    //     if (leftIndex > rightIndex)
    //    //     {
    //    //         int temp = leftIndex;
    //    //         leftIndex = rightIndex;
    //    //         rightIndex = temp;
    //    //     }
    //    // 
    //    //     // Removes the substring.
    //    //     str = str.Remove(leftIndex, Mathf.Abs(rightIndex - leftIndex) + 1);
    //    // 
    //    //     // Find the new indexes.
    //    //     leftIndex = str.IndexOf("<");
    //    //     rightIndex = str.IndexOf(">");
    //    // }

    //    // NEW
    //    // The unformatted string, and the formats to consider.
    //    string unformatted = str;
    //    List<string> formats = new List<string>();

    //    /*
    //     * The symbols to remove, sourced from: https://www.w3schools.com/html/html_formatting.asp
    //     * <b> - Bold text
    //     * <strong> - Important text
    //     * <i> - Italic text
    //     * <em> - Emphasized text
    //     * <mark> - Marked text
    //     * <small> - Smaller text
    //     * <del> - Deleted text
    //     * <ins> - Inserted text
    //     * <sub> - Subscript text
    //     * <sup> - Superscript text
    //     */

    //    // BOLD
    //    formats.Add("<b>");
    //    formats.Add("</b>");

    //    // IMPORTANT/STRING TEXT
    //    formats.Add("<strong>");
    //    formats.Add("</strong>");

    //    // ITALICS
    //    formats.Add("<i>");
    //    formats.Add("</i>");

    //    // EMPHASIZED TEXT
    //    formats.Add("<em>");
    //    formats.Add("</em>");

    //    // MARKED TEXT
    //    formats.Add("<mark>");
    //    formats.Add("</mark>");

    //    // SMALL TEXT
    //    formats.Add("<small>");
    //    formats.Add("</small>");

    //    // DEL TEXT
    //    formats.Add("<del>");
    //    formats.Add("</del>");

    //    // INS TEXT
    //    formats.Add("<ins>");
    //    formats.Add("</ins>");

    //    // SUB TEXT
    //    formats.Add("<sub>");
    //    formats.Add("</sub>");

    //    // SUP TEXT
    //    formats.Add("<sup>");
    //    formats.Add("</sup>");

    //    // Removes all the formats.
    //    foreach(string format in formats)
    //    {
    //        // Remove the format symbol.
    //        unformatted = unformatted.Replace(format, "");
    //    }

    //    // Returns the string with its formatting removed.
    //    return unformatted;
    //}

    //// Gets the length of the string without formatting.
    //public static int GetStringLengthNoFormatting(string str)
    //{
    //    // Gets the string with no formatting, then gets its length and returns it.
    //    string resStr = GetStringNoFormatting(str);
    //    int result = resStr.Length;

    //    // Returns the length.
    //    return result;
    //}
}

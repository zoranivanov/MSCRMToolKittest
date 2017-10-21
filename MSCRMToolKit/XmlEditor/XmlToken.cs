//---------------------------------------------------------------------------
// File Name:      XmlToken.cs
// Description:    Represents an Xml token.
//
// Author:         Ali Badereddin
// Created:        26/12/2009
//---------------------------------------------------------------------------

#region Using Directives

#endregion Using Directives

#region Enums

/// <summary>
/// 
/// </summary>
public enum XmlTokenType
{
    /// <summary>
    /// The none
    /// </summary>
    None,
    /// <summary>
    /// The special character
    /// </summary>
    SpecialChar,
    /// <summary>
    /// The escape
    /// </summary>
    Escape,
    /// <summary>
    /// The comment
    /// </summary>
    Comment,
    /// <summary>
    /// The element
    /// </summary>
    Element,
    /// <summary>
    /// The attribute
    /// </summary>
    Attribute,
    /// <summary>
    /// The value
    /// </summary>
    Value
}

#endregion Enums

/// <summary>
/// Represents an Xml token with a specific type, index in the parsed string
/// and text.
/// </summary>
public class XmlToken
{
    #region Instance Variables

    private string text;            //  Token Text
    private int index;              //  Token Index
    private XmlTokenType type;      //  Token Type

    #endregion Instance Variables

    #region Constructors

    /// <summary>
    /// Empty Constructor
    /// </summary>
    public XmlToken()
    {
    }

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="text"></param>
    /// <param name="index"></param>
    /// <param name="type"></param>
    public XmlToken(string text, int index, XmlTokenType type)
    {
        this.text = text;
        this.index = index;
        this.type = type;
    }

    #endregion Constructors

    #region Properties

    /// <summary>
    /// Gets or sets the text.
    /// </summary>
    /// <value>
    /// The text.
    /// </value>
    public string Text
    {
        set
        {
            this.text = value;
        }
        get
        {
            return this.text;
        }
    }

    /// <summary>
    /// Gets or sets the index.
    /// </summary>
    /// <value>
    /// The index.
    /// </value>
    public int Index
    {
        set
        {
            this.index = value;
        }
        get
        {
            return this.index;
        }
    }

    /// <summary>
    /// Gets or sets the type.
    /// </summary>
    /// <value>
    /// The type.
    /// </value>
    public XmlTokenType Type
    {
        set
        {
            this.type = value;
        }
        get
        {
            return this.type;
        }
    }

    #endregion Properties
}
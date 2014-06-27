#pragma strict

var charactersPerSecond : float = 8.0;
var text : String = "Here is some text to scroll";
var rect : Rect;
 
private var textUsing : String;
private var scrollBasis : String;
private var scrollText : String;
 
private var currChar : int = 0;
private var timer : float = 0.0;
 
function Start () {
    NewText();
}
 
function Update () {
    if (textUsing != text)
        NewText();
 
    var secondsPerCharacter : float = 1.0 / charactersPerSecond;
    if (timer > secondsPerCharacter) {
      var iT : int = Mathf.FloorToInt(timer / secondsPerCharacter);
      currChar = (currChar + iT) % textUsing.Length;
      timer -= iT * secondsPerCharacter;
      scrollText = scrollBasis.Substring(currChar, textUsing.Length);   
    }
    timer += Time.deltaTime;
}
 
function OnGUI() {
	GUI.Label(rect, scrollText);
}
 
function NewText() {
    textUsing = text;
    scrollBasis = textUsing+textUsing;
    currChar = 0;
    scrollText = scrollBasis.Substring(currChar, textUsing.Length);
    timer = 0.0;
}
//function to begin search after enter button is pressed
const keyLength = 11;
//how many characters long that set of values after the "-" flag seem to be
function scanResults(){

  document.getElementById("linkResults").innerHTML="";
  //reset fieldset from previous answer each time button is pressed

  var originalLink=document.getElementById("userInputLink").value;
  //set  variable originalLink to equal the string input by user

  document.getElementById("linkResults").innerHTML+=googleResult(originalLink);
  //takes string, sends it as a parameter to search engine querying function and modifies results to show return string
}


//function that takes the youtube link string and returns results of modified string google searched
function googleResult(string){
  var result = "Invalid";
  //default result invalid
  var flagIndex = 0;
  if(string.includes("youtube.com") || string.includes("youtu.be") || string.includes("s.ytimg.com") || string.includes("i2.ytimg.com")){
    //if a youtube link

    flagIndex = string.indexOf('=');
    result = string.substring(flagIndex + 1, flagIndex + keyLength + 1)

    //begin at "-" flag in url and copy ten characters after

    //// TODO: Google current value of result and return links that show up on regular google search,
    //google images, bing search, duckduckgo and display links as result variable instead with newline characters
    //in between them
  }
  return result;
}

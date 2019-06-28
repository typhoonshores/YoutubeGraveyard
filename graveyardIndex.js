//function to begin search after enter button is pressed
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
  var result = "";

  return result;
}

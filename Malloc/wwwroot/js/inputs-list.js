var addNew = document.getElementById('add-input');

var table = document.getElementById('table');
var tbodyRef = table.getElementsByTagName('tbody')[0];
table.style.display = 'none';

addNew.addEventListener('click', function(e){
    alert('dzieki dziala');
});


function GetLatLng(query)
{
 var inp = document.getElementById("search");
 var xmlhttp = new XMLHttpRequest();
 var url = "https://nominatim.openstreetmap.org/?addressdetails=1&q=" + query + "&format=json&limit=1";
 xmlhttp.onreadystatechange = function()
 {
   if (this.readyState == 4 && this.status == 200)
   {
    var myArr = JSON.parse(this.responseText);
    console.log(myArr[0].lat);
   }
 };
 xmlhttp.open("GET", url, true);
 xmlhttp.send();
}

var parsedData;

GetLatLng("Gdynia");
function printTest(data) {
    // fetch("/js/points-list.json")
    // .then(res => res.json())
    // .then(data => console.log(data))
    // .catch(error => console.log(error));

    parsedData = JSON.parse(data);
    console.log(parsedData.length);
    var tb = document.getElementById('table-body');
    var out = '';

    for (let i = 0; i < parsedData.length; i++) {
        console.log(i)
        var el = parsedData[i];

        out+= 
            "<tr><td>" + el.City + "</td><td>" + el.Street + " " + el.StreetNumber + "</td><td>" + el.PostalCode +"</td><td onclick='onDelete(" + i + ")'>Delete</td></tr>";
    }

    tb.innerHTML = out;
    // parsedData.forEach(element => {
    //     street = element.StreetNumber + ' ' + element.Street;
    //    //GetLatLng(street);
        
    //     addElement(element.City, street, element.PostalCode);
    // });
    table.style.display = 'block';
}


function onDelete(el) {
    console.log(parsedData);
    parsedData.splice(el, 1);
    console.log(parsedData);
    printTest(parsedData);  
}


// function addElement(City, street, postal){
//     var row = tbodyRef.insertRow();
//     var cell1 = row.insertCell(0);
//     var cell2 = row.insertCell(1);
//     var cell3 = row.insertCell(2);
//     var cell4 = row.insertCell(3);
//     cell1.innerHTML = City;
//     cell2.innerHTML = street;
//     cell3.innerHTML = postal;
//     actionButtonsDiv = document.createElement("div");
//     actionButtonsDiv.className = "action-buttons";

//     buttonEdit = document.createElement("div");
//     buttonEdit.className = "add-input";
//     buttonEdit.innerHTML = "Edit";

//     buttonDelete = document.createElement("div");
//     buttonDelete.className = "add-input";
//     buttonDelete.innerHTML = "Delete";

//     actionButtonsDiv.appendChild(buttonEdit);
//     actionButtonsDiv.appendChild(buttonDelete);
//     cell4.appendChild(actionButtonsDiv);


// }

var results = document.getElementById("result");
var fileinput = document.getElementById("file-input");
var jsonFile;
fileinput.addEventListener('input', (event) => {
    const file = event.target.files[0];
    const fileReader = new FileReader();
    fileReader.readAsText(file, 'utf-8');
    fileReader.onload = (event) => {
        // dotnetHelper.dh("AddJSON", event.target.result);
        jsonFile = event.target.result;
        console.log(jsonFile.Street);
        printTest(jsonFile);
    }
});


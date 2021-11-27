var addNew = document.getElementById('add-input');
data = '[{"StreetNumber":"17","Street":"Witolda","PostalCode":"81-532","City":"Gdynia","IsNormalized":true,"OpenTime":"08:00:00","CloseTime":"16:00:00"},{"StreetNumber":"39A","Street":"Witolda","PostalCode":"81-532","City":"Gdynia","IsNormalized":true,"OpenTime":"08:00:00","CloseTime":"16:00:00"},{"StreetNumber":"260","Street":"Wielkoplska","PostalCode":"81-532","City":"Gdynia","IsNormalized":false,"OpenTime":"08:00:00","CloseTime":"16:00:00"},{"StreetNumber":"16A","Street":"Korzenna","PostalCode":"81-587","City":"Gdynia","IsNormalized":true,"OpenTime":"08:00:00","CloseTime":"16:00:00"},{"StreetNumber":"4","Street":"Wiczlinska","PostalCode":"81-578","City":"Gdynia","IsNormalized":false,"OpenTime":"08:00:00","CloseTime":"16:00:00"},{"StreetNumber":"40C","Street":"Starowiejska","PostalCode":"81-356","City":"Gdynia","IsNormalized":true,"OpenTime":"08:00:00","CloseTime":"16:00:00"},{"StreetNumber":"7","Street":"10 Lutego","PostalCode":"81-366","City":"Gdynia","IsNormalized":true,"OpenTime":"08:00:00","CloseTime":"16:00:00"},{"StreetNumber":"46 A-B","Street":"A. Abrahama","PostalCode":"81-395","City":"Gdynia","IsNormalized":false,"OpenTime":"08:00:00","CloseTime":"16:00:00"},{"StreetNumber":"75","Street":"Świętojańska","PostalCode":"81-389","City":"Gdynia","IsNormalized":true,"OpenTime":"08:00:00","CloseTime":"16:00:00"},{"StreetNumber":"","Street":"Swietojanska 75","PostalCode":"81-389","City":"Gdynia","IsNormalized":false,"OpenTime":"08:00:00","CloseTime":"16:00:00"},{"StreetNumber":"45","Street":"Ignacego Krasickiego","PostalCode":"81-377","City":"Gdynia","IsNormalized":true,"OpenTime":"08:00:00","CloseTime":"16:00:00"},{"StreetNumber":"5","Street":"Necla","PostalCode":"81-377","City":"Gdynia","IsNormalized":false,"OpenTime":"08:00:00","CloseTime":"16:00:00"},{"StreetNumber":"24","Street":"B. Chłopskich","PostalCode":"81-415","City":"Gdynia","IsNormalized":false,"OpenTime":"08:00:00","CloseTime":"16:00:00"},{"StreetNumber":"3","Street":"Łużycka","PostalCode":"81-537","City":"Gdynia","IsNormalized":true,"OpenTime":"10:00:00","CloseTime":"16:00:00"},{"StreetNumber":"3A","Street":"Luzycka","PostalCode":"81-537","City":"Gdynia","IsNormalized":false,"OpenTime":"08:00:00","CloseTime":"16:00:00"},{"StreetNumber":"4A","Street":"plac Górnośląski","PostalCode":"81-509","City":"Gdynia","IsNormalized":true,"OpenTime":"08:00:00","CloseTime":"16:00:00"},{"StreetNumber":"238","Street":"aleja Zwycięstwa","PostalCode":"81-540","City":"Gdynia","IsNormalized":true,"OpenTime":"08:00:00","CloseTime":"16:00:00"},{"StreetNumber":"899","Street":"aleja Niepodległości","PostalCode":"81-861","City":"Sopot","IsNormalized":true,"OpenTime":"09:00:00","CloseTime":"17:00:00"},{"StreetNumber":"938","Street":"al. Niepodległości","PostalCode":"81-861","City":"Sopot","IsNormalized":false,"OpenTime":"08:00:00","CloseTime":"16:00:00"},{"StreetNumber":"59","Street":"Haffnera","PostalCode":"81-715","City":"Sopot","IsNormalized":false,"OpenTime":"08:00:00","CloseTime":"16:00:00"},{"StreetNumber":"42","Street":"Jana Jerzego Haffnera","PostalCode":"81-708","City":"Sopot","IsNormalized":true,"OpenTime":"08:00:00","CloseTime":"16:00:00"},{"StreetNumber":"42","Street":"Jana Jerzego Haffnera","PostalCode":"81-708","City":"Sopot","IsNormalized":true,"OpenTime":"08:00:00","CloseTime":"16:00:00"},{"StreetNumber":"9","Street":"Michała Drzymały","PostalCode":"81-771","City":"Sopot","IsNormalized":true,"OpenTime":"08:00:00","CloseTime":"16:00:00"},{"StreetNumber":"2","Street":"Poniatowskiego","PostalCode":"81-724","City":"Sopot","IsNormalized":false,"OpenTime":"10:00:00","CloseTime":"16:00:00"},{"StreetNumber":"54","Street":"Bitwy Pod Płowcami","PostalCode":"81-731","City":"Sopot","IsNormalized":true,"OpenTime":"08:00:00","CloseTime":"16:00:00"},{"StreetNumber":"27","Street":"Kościuszki","PostalCode":"80-445","City":"Gdańsk","IsNormalized":false,"OpenTime":"08:00:00","CloseTime":"16:00:00"},{"StreetNumber":"9","Street":"Jana Kilińskiego","PostalCode":"80-452","City":"Gdańsk","IsNormalized":true,"OpenTime":"08:00:00","CloseTime":"16:00:00"},{"StreetNumber":"4C/1","Street":"Danusi","PostalCode":"80-434","City":"Gdańsk","IsNormalized":true,"OpenTime":"08:00:00","CloseTime":"16:00:00"},{"StreetNumber":"20/2","Street":"Wyspiańskiego","PostalCode":"80-434","City":"Gdańsk","IsNormalized":false,"OpenTime":"08:00:00","CloseTime":"16:00:00"},{"StreetNumber":"49","Street":"al. Grunwaldzka","PostalCode":"80-241","City":"Gdańsk","IsNormalized":false,"OpenTime":"08:00:00","CloseTime":"16:00:00"},{"StreetNumber":"56","Street":"aleja Grunwaldzka","PostalCode":"80-241","City":"Gdańsk","IsNormalized":true,"OpenTime":"08:00:00","CloseTime":"16:00:00"},{"StreetNumber":"12","Street":"aleja Grunwaldzka","PostalCode":"80-236","City":"Gdańsk","IsNormalized":true,"OpenTime":"12:00:00","CloseTime":"18:00:00"},{"StreetNumber":"115C","Street":"Traugutta","PostalCode":"80-226","City":"Gdańsk","IsNormalized":true,"OpenTime":"08:00:00","CloseTime":"16:00:00"},{"StreetNumber":"65","Street":"Do Studzienki","PostalCode":"80-227","City":"Gdańsk","IsNormalized":true,"OpenTime":"08:00:00","CloseTime":"16:00:00"},{"StreetNumber":"24","Street":"Beethovena","PostalCode":"80-001","City":"Gdańsk","IsNormalized":true,"OpenTime":"09:00:00","CloseTime":"17:00:00"},{"StreetNumber":"187","Street":"Kartuska","PostalCode":"80-122","City":"Gdańsk","IsNormalized":true,"OpenTime":"08:00:00","CloseTime":"16:00:00"},{"StreetNumber":"103","Street":"Malczewskiego","PostalCode":"80-107","City":"Gdańsk","IsNormalized":false,"OpenTime":"08:00:00","CloseTime":"16:00:00"},{"StreetNumber":"35","Street":"Stoczniowców","PostalCode":"80-812","City":"Gdańsk","IsNormalized":true,"OpenTime":"08:00:00","CloseTime":"16:00:00"},{"StreetNumber":"35","Street":"trakt Świętego Wojciecha","PostalCode":"80-812","City":"Gdańsk","IsNormalized":true,"OpenTime":"08:00:00","CloseTime":"16:00:00"},{"StreetNumber":"3","Street":"Olsztyńska","PostalCode":"80-734","City":"Gdańsk","IsNormalized":true,"OpenTime":"08:00:00","CloseTime":"16:00:00"},{"StreetNumber":"29","Street":"Olsztyńska","PostalCode":"80-734","City":"Gdańsk","IsNormalized":true,"OpenTime":"08:00:00","CloseTime":"16:00:00"},{"StreetNumber":"32","Street":"Miodowa","PostalCode":"80-738","City":"Gdańsk","IsNormalized":true,"OpenTime":"08:00:00","CloseTime":"16:00:00"},{"StreetNumber":"35","Street":"Zawodzie","PostalCode":"80-726","City":"Gdańsk","IsNormalized":true,"OpenTime":"12:00:00","CloseTime":"18:00:00"},{"StreetNumber":"43","Street":"Wschodnia","PostalCode":"80-066","City":"Gdańsk","IsNormalized":true,"OpenTime":"08:00:00","CloseTime":"16:00:00"},{"StreetNumber":"6D","Street":"Strzelców Karpackich","PostalCode":"80-041","City":"Gdańsk","IsNormalized":true,"OpenTime":"08:00:00","CloseTime":"16:00:00"},{"StreetNumber":"18B","Street":"Dywizji Wołyńskiej","PostalCode":"80-180","City":"Gdańsk","IsNormalized":true,"OpenTime":"08:00:00","CloseTime":"16:00:00"},{"StreetNumber":"18B","Street":"Dywizji Wołyńskiej","PostalCode":"80-180","City":"Gdańsk","IsNormalized":true,"OpenTime":"08:00:00","CloseTime":"16:00:00"},{"StreetNumber":"60","Street":"Swietokrzyska","PostalCode":"80-180","City":"Gdańsk","IsNormalized":false,"OpenTime":"08:00:00","CloseTime":"16:00:00"},{"StreetNumber":"22","Street":"Świętokrzyska","PostalCode":"80-180","City":"Gdańsk","IsNormalized":true,"OpenTime":"08:00:00","CloseTime":"16:00:00"},{"StreetNumber":"32","Street":"Niepołomnicka","PostalCode":"80-180","City":"Gdańsk","IsNormalized":true,"OpenTime":"08:00:00","CloseTime":"16:00:00"}]';
var table = document.getElementById('table');
var tbodyRef = table.getElementsByTagName('tbody')[0];
table.style.display = 'none';

addNew.addEventListener('click', function(e){
    alert('dzieki dziala');
});


function printTest() {
    fetch("/js/points-list.json")
    .then(res => res.json())
    .then(data => console.log(data))
    .catch(error => console.log(error));
    
    var parsedData = JSON.parse(data);
    parsedData.forEach(element => {
        street = element.Street + ' ' + element.StreetNumber;
        addElement(element.City, street, element.PostalCode);
    });
    table.style.display = 'block';
}

function addElement(City, street, postal){
    var row = tbodyRef.insertRow();
    var cell1 = row.insertCell(0);
    var cell2 = row.insertCell(1);
    var cell3 = row.insertCell(2);
    var cell4 = row.insertCell(3);
    cell1.innerHTML = City;
    cell2.innerHTML = street;
    cell3.innerHTML = postal;
    actionButtonsDiv = document.createElement("div");
    actionButtonsDiv.className = "action-buttons";

    buttonEdit = document.createElement("div");
    buttonEdit.className = "add-input";
    buttonEdit.innerHTML = "Edit";

    buttonDelete = document.createElement("div");
    buttonDelete.className = "add-input";
    buttonDelete.innerHTML = "Delete";

    actionButtonsDiv.appendChild(buttonEdit);
    actionButtonsDiv.appendChild(buttonDelete);
    cell4.appendChild(actionButtonsDiv);


}


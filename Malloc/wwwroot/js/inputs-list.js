var addNew = document.getElementById('add-input');

var table = document.getElementById('table');
var tbodyRef = table.getElementsByTagName('tbody')[0];
table.style.display = 'none';

addNew.addEventListener('click', function(e){
    alert('dzieki dziala');
});

var parsedData;

function printTest(data) {
    parsedData = JSON.parse(data);
    createTable(parsedData);
}


const createTable = (data) => {
    var tb = document.getElementById('table-body');
    var out = '';

    for (let i = 0; i < data.length; i++) {
        console.log(i)
        var el = data[i];

        out+= 
            "<tr><td>" + el.City + "</td><td>" + el.Street + " " + el.StreetNumber + "</td><td>" + el.PostalCode +"</td><td style='text-align:center;' onclick='onDelete(" + i + ")'><i class='far fa-times-circle'></i></td></tr>";
    }

    tb.innerHTML = out;
    table.style.display = 'block';
}

function onDelete(el) {
    parsedData.splice(el, 1);
    createTable(parsedData);
}

var results = document.getElementById("result");
var fileinput = document.getElementById("file-input");
var jsonFile;

fileinput.addEventListener('input', (event) => {
    const file = event.target.files[0];
    const fileReader = new FileReader();

    fileReader.readAsText(file, 'utf-8');

    fileReader.onload = (event) => {
        jsonFile = event.target.result;
        printTest(jsonFile);
    }
});


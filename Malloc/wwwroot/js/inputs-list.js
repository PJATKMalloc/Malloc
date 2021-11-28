
var table = document.getElementById('table');
var tbodyRef = table.getElementsByTagName('tbody')[0];
table.style.display = 'none';

var parsedData = [];

function addElement() {
    const newElement = new Object();
        newElement.StreetNumber = "938",
        newElement.Street = "al. Niepodległości",
        newElement.PostalCode = "81-861",
        newElement.City = "Sopot",
        newElement.OpenTime = "08:00:00",
        newElement.CloseTime = "16:00:00"
        console.log(newElement);
        console.log(parsedData);
        parsedData.push(newElement);


        // if (parsedData.length > 0) {
        //     console.log("istnieje tablica");
        // }else{
        //     newArray = [];
        //     newArray.push(newElement);
        // }
        createTable(parsedData);
    };

function printTest(data) {
    console.log("Przed");
    console.log(parsedData);

    JSON.parse(data).forEach(element => {
        parsedData.push(element);
    });
    //parsedData.push(JSON.parse(data));
    console.log("PO");
    console.log(parsedData);
    createTable(parsedData);
}


const createTable = () => {
    var tb = document.getElementById('table-body');
    var out = '';

    for (let i = 0; i < parsedData.length; i++) {
        console.log(i)
        var el = parsedData[i];

        out +=
            "<tr><td>" + el.City + "</td><td style='width:100%'>" + el.Street + " " + el.StreetNumber + "</td><td>" + el.PostalCode + "</td><td style='text-align:center;' onclick='onDelete(" + i + ")'><i class='far fa-times-circle'></i></td></tr>";
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

var arrow1 = document.getElementById('arrow-1');
var arrow2 = document.getElementById('arrow-2');
var sidebar = document.getElementById('sidebar');
var sidebar2 = document.getElementById('sidebar-2');
arrow1.addEventListener('click', () => {
    sidebar.classList.remove('fadeIn');
    sidebar2.classList.remove('fadeOut');
    sidebar.classList.add('fadeOut');
    sidebar2.classList.add('fadeIn');
});
arrow2.addEventListener('click', () => {
    sidebar2.classList.remove('fadeIn');
    sidebar.classList.remove('fadeOut');
    sidebar2.classList.add('fadeOut');
    sidebar.classList.add('fadeIn');
});


// objects store data in key-value pairs

let student = {
  name: "Ayush Shukla",
  age: 21,
  isAdmin: true,
  courses: ["html", "css", "js"],
};

//bracket is used for dynamic access

console.log(student.name);
console.log(student["courses"]);

/*
Json vs Object
JSON is a string representation of data while Object is a data structure in JavaScript.
Objects can contain methods (functions) while JSON cannot.


Conversion:

let jsonString = '{"name": "Ayush", "age": 21, "isAdmin": true}';

let obj = JSON.parse(jsonString); // JSON to Object

let newJsonString = JSON.stringify(obj); // Object to JSON
*/

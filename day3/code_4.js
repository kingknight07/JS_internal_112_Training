/*
arr = ["apple", "banana", "cherry", "date"];

for(let value of arr){ 
    console.log(value);
}

*/

let numbers = [1,2,3]

let doubled = numbers.map(n => n*2);

console.log(doubled);

console.log("\n");

let nums = [10,25,30,40];
let filtered = nums.filter(n => n > 25);

console.log(filtered);


console.log("\n");


//array.reduce((accumulator, currentValue) => {...} , initialValue)
//accumulator stores the result as loop continues
//currentvalue is the current element being processed
//initialValue is the starting value for accumulator

let n = [1,2,3,4,5,6,7,8,9,10];
let sum = n.reduce((acc, curr) => acc + curr, 0);

console.log(sum);

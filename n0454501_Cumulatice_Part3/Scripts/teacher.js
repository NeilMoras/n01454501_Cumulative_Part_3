


function UpdateTeacher(TeacherId){
// goal: send a request which expample
//POST: http:// localhost:51326/api/TeacherData/UpdateTeacher/{id}
//with POST data of teachefname, teacherlname,  employeenumber,hiring date and salary


var URL = "http://localhost:52064/api/TeacherData/UpdateTeacher/" +TeacherId;


var request = new XMLHttpRequest();
// where about the request is sent to?
//

var TeacherFname = document.getElementById('TeacherFname').value;
var TeacherLname = document.getElementById('TeacherLname').value;
var EmployeeNumber = document.getElementById('EmployeeNumber').value;
var HireDate = document.getElementById('HireDate').value;
var Salary = document.getElementById('Salary').value;


var TeacherData = {
  "TeacherFname" : TeacherFname,
  "TeacherLname" : TeacherFname,
  "EmployeeNumber": EmployeeNumber,
  "HireDate": HireDate

};


request.open("POST",URL,true);
request.setRequestHeader("Content-Type", "application/json");
request.onreadystatechange = function(){

  if(request.readyState === 4 && request.status === 200){
  //request is successful and the request is finished

  // nothing to render, the method returns nothing

  }
}
//POST information send through . send() method
request.send(JSON.stringify(TeacherData));

}

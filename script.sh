#!/bin/bash 
rm test test2 timetablelink header timetablelink2 timetable list cjar list2 listdump listtimetable cal.csv
echo "http://allocate.swin.edu.au/aplus/apstudent?fun=" >> test
echo "http://allocate.swin.edu.au/aplus/" >> timetablelink
echo "http://allocate.swin.edu.au/aplus/" >> list
curl --cookie-jar cjar --output /dev/null http://allocate.swin.edu.au/aplus/apstudent\?fun\=login
curl --cookie cjar --cookie-jar cjar --data "student_code=$1" --data "password=$2" --data 'form_id=form1' http://allocate.swin.edu.au/aplus/apstudent\?fun\=login |  grep -oe "header&ss=[0-9]*" >> test
tr -d '\n' < test >> test2
curl --cookie cjar --cookie-jar cjar `cat test2` --output header
cat header | grep -oe "apstudent?fun=show_all_alloc[^\"]*" >> timetablelink
tr -d '\n' < timetablelink >> timetablelink2
curl --cookie cjar --cookie-jar cjar `cat timetablelink2` --output timetable
cat timetable | grep -oe "apstudent?fun=show_all_allocations&[^\"]*" >> list
tr -d '\n' < list >> list2
curl --cookie cjar --cookie-jar cjar `cat list2` --output listtimetable
lynx -dump listtimetable >> listdump
#tidy listdump >> listdumptidy
node parse.js >> cal.csv
rm test test2 timetablelink header timetablelink2 timetable list cjar list2 listdump listtimetable

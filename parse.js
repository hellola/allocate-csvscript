var sys = require('sys');
var htmlparser = require('htmlparser');
var fs = require('fs');
var jsdom = require('jsdom');
fs.readFile('listdump','utf8',function(err,data) {
    if (err) {
        return console.log(err);
    }
    var htmlData = data;
    jsdom.env({
        html: htmlData,
    scripts: [
      'http://code.jquery.com/jquery-1.5.min.js'
    ]
  }, function (err, window) {
    var $ = window.jQuery;
    var tables = $('table');
    var i = 0;
    var r = 0;
    var table = '';
    var csv = createHeader();
    tables.each(function() {
        if (i == 6) {
            $(this).children('tr').each(function() {
                var line = '';
                $(this).children('td').each(function() {
                    line += $(this).text()+',';
                });
                table += line.replace(/,$/g,'')+ '\n';
                var opts = line.split(',');
                var time = opts[5];
                var day = opts[4];
                var desc = opts[0]; 
                var loc = opts[7];
                var dur = opts[9];
                var weeks = opts[10];
                if (r != 0) {
                    csv += createEvent(time,day,desc,loc,dur,weeks);
                }
                r++;
                
            });
    console.log(csv);
    }
    i++;
    });

  });
});

function formatDate(date) {
    var d = date.split('/');
    return d[1] + '/' + d[0] + '/' + d[2];
}

function formatTime(time) { 
    var t = time.split(':');
    var sHour = t[0];
    var hour = parseInt(sHour,10);
    var ampm = 'AM';
    if (hour >= 12 ) {
        ampm = 'PM';
        hour = hour % 12;
        if (hour == 0) { 
            hour = 12;
        }
    }
    if (hour.toString().length == 1)  { 
        hour = '0'+hour;
    }
    time = hour + ':' + t[1]+ ':00' + ' ' + ampm;
    return time;
    
}

function createEvent(time,day,desc,loc,dur,weeks) {
    desc = desc.split('\n')[1];
    var startDate = weeks.split(',')[0].split('-')[0] + '/12';
    startDate = formatDate(startDate);
    var toTime = time;
    var arrToTime = toTime.split(':');
    arrToTime[0] = ((parseInt(dur)/60)+parseInt(arrToTime[0],10)).toString();
    toTime = arrToTime[0] + ':' + arrToTime[1];
    return desc + ','+startDate + ','+formatTime(time)+ ','+startDate+','+formatTime(toTime)+','+desc+',' + loc+',True'+'\n';
}

function createHeader() {
    return 'Subject,Start Date,Start Time,End Date,End Time,Description,Location,Private\n';
}

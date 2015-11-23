var measure = angular.module('measure', []), start = new Date(), measureNotes = {}, notes = [], isInit = true, masterSong;


measure.controller('MeasureCtrl', function ($scope, $http) {
    $http.get('home/getnotes/', {
        params: {
            start: start.toISOString(),
            songName:"a_train"
        }
    }).success(function (data) {
        $scope.notes = $scope.notes || [];
        for (note in data.Notes) {
            $scope.notes.push({ note: data.Notes[note].Value, duration: data.Notes[note].Duration, soloNote : 'note_' + data.Notes[note].SoloValue });
        }
        console.log($scope.notes);
        if (isInit) initPlay('noteHolder');
        isInit = false;
    });
});


function initPlay(notes) {
    
    
    function play(audio, prevAudio, callback) {
        if (prevAudio != null)
            prevAudio.removeClass('active').addClass('used').children('audio').trigger('pause').siblings('p').stop();
        //console.log(prevAudio);
        
        console.log(audio.children('audio').attr('src'));
        audio.removeClass('inactive').addClass('active').children('audio').trigger('play');
        audio.animate({ 'left': '-=2000px' }, 6000, 'linear');


        audio.children('p').css('background-color', 'rgba(' + Math.floor(Math.random() * 255) + ',' + Math.floor(Math.random() * 255) + ',' + Math.floor(Math.random() * 255) + ', 0.3)')
            .animate({
            borderTopLeftRadius: '50%',
            borderTopRightRadius: '50%',
            borderBottomLeftRadius: '50%',
            borderBottomRightRadius: '50%',
            height: 3000,
            width: 3000,
            top: -1455,
            left:-1200
        }, 5000, 'linear');
        

        if (callback) {
            //When the audio object completes it's playback, call the callback
            //provided      
            callback.call();
        }
    }

    //Changed the name to better reflect the functionality
    function play_sound_queue(notes) {
        var index = 0;
        var noteQueue = document.getElementsByClassName(notes);
        function canIPlayYet() {
            if (noteQueue.length > 0)
                recursive_play();
            else
                setTimeout(canIPlayYet, 500);
        }
        canIPlayYet();
        function recursive_play() {
            //If the index is the last of the table, play the sound
            //without running a callback after    
            if (index + 1 === noteQueue.length) {
                play($(noteQueue[index]), null);
            }
            else {
                //Else, play the sound, and when the playing is complete
                //increment index by one and play the sound in the 
                //indexth position of the array
                setTimeout(function () {
                    play($(noteQueue[index]), $(noteQueue[index - 1]), function () { index++; recursive_play() });
                }, $(noteQueue[index]).children('audio').attr('data-ng-duration')); 
            }
        }
    }

    function play_all(notes) {
        $('#masterSong').trigger('play');
        play_sound_queue(notes);
    }


    play_all(notes);

}
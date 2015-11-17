var measure = angular.module('measure', []), start = new Date(), measureNotes = {}, notes = [], isInit = true, masterSong;


measure.controller('MeasureCtrl', function ($scope, $http, $q) {
    $q.when()
        .then(function () {
            var deferred = $q.defer();
            $http.get('/Assets/json/a_train.json')
                .success(function (data) {
                    deferred.resolve(data);
                });
            return deferred.promise;
        })
        .then(function (song) {
            masterSong = song;
            for (chord in song.measures) {
                    $http.get('home/measure', {
                        params: {
                            start: start.toISOString(),
                            bpm: song.bpm,
                            tsN: song.tsN,
                            tsD: song.tsD,
                            measureFraction: chord.duration,
                            root: chord.root,
                            quality: chord.quality,
                            auxiliary: chord.auxiliary
                        }
                    }).success(function (data) {
                        $scope.notes = $scope.notes || [];
                        for (note in data.Notes) {
                            $scope.notes.push({ note: new Audio('Assets/chrom_sax/' + data.Notes[note].Value + '.mp3'), duration: data.Notes[note].Duration });
                        }
                        if (isInit) initPlay($scope.notes);
                        isInit = false;
                    });
            }
        });
});



function initPlay(notes) {
    
    function play(audio, callback) {
        var noteRep = '<li class="active"><span style="background:' + '#' + Math.floor(Math.random() * 16777215).toString(16) + '"></span></li>';
        $('.noteHolder').children('li').removeClass('active').addClass('notActive');
        $('.noteHolder li.notActive span').stop();
        $('.noteHolder').append(noteRep);
        //$('.noteHolder li.active span').animate({
        //    borderTopLeftRadius: '50%',
        //    borderTopRightRadius: '50%',
        //    borderBottomLeftRadius: '50%',
        //    borderBottomRightRadius: '50%',
        //    width: 3000,
        //    height: 3000,
        //}, 2000);
        $('.noteHolder li').animate({ left:"-=2000px" }, 6000);
        audio.note.play();
        audio.note.volume = 0.3;
        if (callback) {
            //When the audio object completes it's playback, call the callback
            //provided      
            setTimeout(function () {
                callback.call();
                audio.note.pause();
            }, (audio.duration.decimalDuration));
        }
    }

    //Changed the name to better reflect the functionality
    function play_sound_queue(sounds) {
        var index = 0;
        function recursive_play() {
            //If the index is the last of the table, play the sound
            //without running a callback after       
            if (index + 1 === sounds.length) {
                play(sounds[index], null);
            }
            else {
                //Else, play the sound, and when the playing is complete
                //increment index by one and play the sound in the 
                //indexth position of the array
                play(sounds[index], function () { index++; recursive_play(); });
            }
        }

        //Call the recursive_play for the first time
        recursive_play();
    }

    function play_all(notes) {
        $('#masterSong').trigger('play');
        var songIntro = new Date();
        songIntro.setSeconds(songIntro.getSeconds() + masterSong.intro);
        function canIPlayNotesYet() {
            var newDate = new Date();
            if (songIntro >= newDate) {
                play_sound_queue(notes);
            }
            else {
                setTimeout(function () { canIPlayNotesYet() }, 1000);
            }
        }
        canIPlayNotesYet();
    }


    play_all(notes);

}

// Write your Javascript code.

//$(document).ready(function () {
//    var start = new Date(), measure = {}, notes = [], isInit = true;

//    $.ajax({
//        url: '/Assets/json/a_train.json',
//        dataType: 'json',
//        async: false,
//        success: function (data) {
//            console.log(data);
//            song = data;
//        }
//    });

//    for (chord in song.measures)
//    {
//        setTimeout(function () {
//            $.ajax({
//                type: "GET",
//                contentType: "application/json; charset=utf-8",
//                url: '/Home/Measure',
//                dataType: "json",
//                data: { start: start.toISOString(), bpm: song.bpm, tsN: song.tsN, tsD: song.tsD, measureFraction: chord.duration, root: chord.root, quality: chord.quality, auxiliary: chord.auxiliary },
//                success: function (data) {
//                    start.setSeconds(start.getSeconds() + 4);
//                    measure = data;

//                    for (note in data.Notes) {
//                        console.log(data.Notes[note].Value)
//                        notes.push({ note: new Audio('/Assets/chrom_sax/' + data.Notes[note].Value + '.mp3'), duration: data.Notes[note].Duration });
//                    }
//                    console.log(notes);
//                    if (isInit) initPlay();
//                    isInit = false;
//                },
//            });
//        }, 1000);
//    }
    
    

//    function initPlay(songIntro) {
//        function play(audio, callback) {
//            console.log(audio.note);
//            console.log(audio.duration);
//            audio.note.play();
//            audio.note.volume = 0.3;
//            if (callback) {
//                When the audio object completes it's playback, call the callback
//                provided      
//                setTimeout(function () {
//                    callback.call();
//                    audio.note.pause();
//                }, (audio.duration));
//            }
//        }

//        Changed the name to better reflect the functionality
//        function play_sound_queue(sounds) {
//            var index = 0;
//            function recursive_play() {
//                If the index is the last of the table, play the sound
//                without running a callback after       
//                if (index + 1 === sounds.length) {
//                    play(sounds[index], null);
//                }
//                else {
//                    Else, play the sound, and when the playing is complete
//                    increment index by one and play the sound in the 
//                    indexth position of the array
//                    play(sounds[index], function () { index++; recursive_play(); });
//                }
//            }

//            Call the recursive_play for the first time
//            recursive_play();
//        }

//        function play_all() {
//            $('#masterSong').trigger('play');
//            var songIntro = new Date();
//            songIntro.setSeconds(songIntro.getSeconds() + song.intro);
//            function canIPlayNotesYet() {
//                var newDate = new Date();
//                if (songIntro >= newDate) {

//                    play_sound_queue(notes);
//                }
//                else {
//                    setTimeout(function () { canIPlayNotesYet() }, 1000);
//                }
//            }
//            canIPlayNotesYet();
//        }

        
//        play_all();

//    }
//});
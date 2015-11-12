// Write your Javascript code.

$(document).ready(function () {
    var start = new Date(), measure = {}, notes = [];

    $.ajax({
        type: "GET",
        contentType: "application/json; charset=utf-8",
        url: '/Home/Measure',
        dataType: "json",
        data: { start: start.toISOString(), bpm : 140, tsN : 4, tsD: 4, root : "c", quality: "major", auxiliary : "6" },
        success: function (data) {
            start.setSeconds(start.getSeconds() + 4);
            measure = data;

            for (note in data.Notes) {
                console.log(data.Notes[note].Value)
                notes.push({ note: new Audio('/Assets/chrom_sax/' + data.Notes[note].Value + '.mp3'), duration: data.Notes[note].Duration });
            }

            initPlay();
        },
    });

    function initPlay() {
        function play(audio, callback) {
            console.log(audio.note);
            console.log(audio.duration);
            audio.note.play();

            if (callback) {
                //When the audio object completes it's playback, call the callback
                //provided      
                setTimeout(function () {
                    callback.call();
                    audio.note.pause();
                }, (audio.duration));
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

        function play_all() {
            play_sound_queue(notes);
        }

        play_all();


    }
});
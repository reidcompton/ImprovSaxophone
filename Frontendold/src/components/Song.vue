<template>
  <div class="songWrapper">
    <audio id="masterSong" src="/a_train.mp3" type="audio/mpeg"></audio>
    <div class="staff">
      <div></div>
      <div></div>
      <div></div>
      <div></div>
      <div></div>
    </div>
    <div id="notesWrapper">
      <div class="noteHolder inactive" v-bind:class="('note_' + note.note)" v-for="note in notes" v-bind:key="note.id">
        <audio class="noteQueue" v-bind:data-duration="note.duration.doubleDuration">
            <source v-bind:src="'/chrom_sax/' + note.note + '.mp3'" type="audio/mpeg">
        </audio>
        <p class="note" v-bind:class="(note.soloOctave == 4 ? 'strikethrough' : '')">
          <span>&#9834;</span>
        </p>
      </div>
    </div>
    <div class="start-btn">
        <button v-on:click="play_all">Start</button>
    </div>
  </div>
</template>

<script>
import datalayer from '../datalayer';
global.jQuery = require('jquery');
var $ = global.jQuery;
window.$ = $;
export default {
  name: 'Song',
  data() {
      return {
        start: new Date(), 
        measureNotes: {}, 
        notes: [], 
        masterSong: null
      }
  },
  methods: {
    animate(element, duration) {
        let start = Date.now();
        let timer = setInterval(function() {
            let timePassed = Date.now() - start;

            element.style.left = timePassed / 5 + 'px';

            if (timePassed > duration) clearInterval(timer);

        }, 20);
    },
    play(audio, prevAudio, callback) {
          if (prevAudio != null) {
            // prevAudio.classList.remove('active')
            // prevAudio.classList.add('used')
            // prevAudio.querySelector('audio').pause()
            // prevAudio.querySelector('p');//.stop(); 
            $(prevAudio).removeClass('active').addClass('used').children('audio').trigger('pause').siblings('p').stop();
          }
            
        //   audio.classList.remove('inactive')
        //   audio.classList.add('active')
        //   audio.querySelector('audio').play();
        //   this.animate(audio, 6000);
        $(audio).removeClass('inactive').addClass('active').children('audio').trigger('play');
        $(audio).animate({ 'left': '-=2000px' }, 6000, 'linear');


          $(audio).children('p').css('background-color', 'rgba(' + Math.floor(Math.random() * 255) + ',' + Math.floor(Math.random() * 255) + ',' + Math.floor(Math.random() * 255) + ', 0.3)')
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
      },
      canIPlayYet(noteQueue, index) {
        if (noteQueue.length > 0)
            this.recursive_play(noteQueue, index);
        else
            setTimeout(this.canIPlayYet(noteQueue, index), 500);
      },
      //Changed the name to better reflect the functionality
      play_sound_queue() {
          var index = 0;
          var noteQueue = document.getElementsByClassName('noteHolder');
          
          this.canIPlayYet(noteQueue, index);
          
      },
      recursive_play(noteQueue, index) {
        //If the index is the last of the table, play the sound
        //without running a callback after    
        if (index + 1 === noteQueue.length) {
            this.play(noteQueue[index], null);
        }
        else {
            //Else, play the sound, and when the playing is complete
            //increment index by one and play the sound in the 
            //indexth position of the array
            setTimeout(function () {
                this.play(noteQueue[index], noteQueue[index - 1], () => { index++; this.recursive_play(noteQueue, index) });
            }.bind(this), noteQueue[index].querySelector('audio').dataset.duration); 
        }
      },
      play_all() {
            
            var playPromise = document.getElementById('masterSong').play();

                // In browsers that don’t yet support this functionality,
                // playPromise won’t be defined.
                if (playPromise !== undefined) {
                playPromise.then(function() {
            
                    // Automatic playback started!
                }).catch(function() {
                
                    // Automatic playback failed.
                    // Show a UI element to let the user manually start playback.
                });
                }
            
            //document.getElementById('masterSong').play();
            this.play_sound_queue();
      }
  },
  mounted() {
    datalayer.get(this.start, "a_train")
    .then((response) => response.json())
    .then((responseJSON) => {
        var i = 0;
        responseJSON.notes.forEach(function(note) {
            this.notes.push({ id: i, note: note.value, duration: note.duration, soloNote : 'note_' + note.soloValue, soloOctave: parseInt(note.soloOctave) });
            i++;
        }.bind(this));
    });
  }
}
</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped>
.start-btn {
    position:relative;
    
    top:100px;
}
.songWrapper,
.staff {
    position: absolute;
    width: 100%;
    top: 50%;
    left: 0
}

.noteHolder,
.songWrapper,
.staff {
    position: absolute
}

body {
    padding-top: 50px;
    padding-bottom: 20px
}

.body-content {
    padding-left: 15px;
    padding-right: 15px
}

input,
select,
textarea {
    max-width: 280px
}

.carousel-caption {
    z-index: 10!important
}

.carousel-caption p {
    font-size: 20px;
    line-height: 1.4
}

@media (min-width:768px) {
    .carousel-caption {
        z-index: 10!important
    }
}

.staff {
    margin-top: -50px;
    height: 100px
}

.staff>div {
    width: 100%;
    height: 2px;
    background: #000;
    margin-top: 20px
}

.noteHolder {
    left: 50%;
    top: 45%
}

.noteHolder.inactive {
    display: none
}

.noteHolder.active {
    display: block
}

.noteHolder .note {
    width: 100px;
    height: 100px;
    border-radius: 50%;
    background: rgba(72, 0, 200, .3);
    position: relative;
    font-size: 70px;
    display: table-cell;
    vertical-align: middle;
    text-align: center
}

.note_asharp_2 p span::before,
.note_asharp_3 p span::before,
.note_asharp_4 p span::before,
.note_csharp_2 p span::before,
.note_csharp_3 p span::before,
.note_csharp_4 p span::before,
.note_dsharp_2 p span::before,
.note_dsharp_3 p span::before,
.note_dsharp_4 p span::before,
.note_fsharp_2 p span::before,
.note_fsharp_3 p span::before,
.note_fsharp_4 p span::before,
.note_gsharp_2 p span::before,
.note_gsharp_3 p span::before,
.note_gsharp_4 p span::before {
    content: '♯';
    font-size: 28px;
    position: absolute;
    left: 50%;
    top: 50%;
    margin-left: -30px;
    margin-top: -20px
}

.note_a_4 p span::after,
.note_asharp_4 p span::after,
.note_b_4 p span::after,
.note_c_2 p span::after,
.note_c_4 p span::after,
.note_csharp_2 p span::after,
.note_csharp_4 p span::after,
.note_d_4 p span::after,
.note_dsharp_4 p span::after,
.note_e_4 p span::after,
.note_f_4 p span::after,
.note_fsharp_4 p span::after,
.note_g_4 p span::after,
.note_gsharp_4 p span::after {
    border-bottom: 3px solid #000;
    content: "";
    line-height: 1em;
    vertical-align: middle;
    display: table-cell;
    position: absolute;
    right: 0;
    top: 50%;
    width: 30px;
    text-align: center;
    left: 50%;
    margin-left: -23px
}

.note_a_4 p span::after,
.note_asharp_4 p span::after,
.note_c_2 p span::after,
.note_c_4 p span::after,
.note_csharp_2 p span::after,
.note_csharp_4 p span::after,
.note_e_4 p span::after,
.note_g_4 p span::after,
.note_gsharp_4 p span::after {
    margin-top: 15px
}

.note_b_4 p span::after,
.note_d_4 p span::after,
.note_dsharp_4 p span::after,
.note_f_4 p span::after,
.note_fsharp_4 p span::after {
    margin-top: 23px
}

.note {
    position: relative
}

.note_c_2,
.note_csharp_2 {
    top: 9px
}

.note_d_2,
.note_dsharp_2 {
    top: 1px
}

.note_e_2 {
    top: -7px
}

.note_f_2,
.note_fsharp_2 {
    top: -18px
}

.note_g_2,
.note_gsharp_2 {
    top: -29px
}

.note_a_2,
.note_asharp_2 {
    top: -40px
}

.note_b_2 {
    top: -54px
}

.note_c_3,
.note_csharp_3 {
    top: -62px
}

.note_d_3,
.note_dsharp_3 {
    top: -74px
}

.note_e_3 {
    top: -84px
}

.note_f_3,
.note_fsharp_3 {
    top: -96px
}

.note_g_3,
.note_gsharp_3 {
    top: -104px
}

.note_a_3,
.note_asharp_3 {
    top: -112px
}

.note_b_3 {
    top: -120px
}

.note_c_4,
.note_csharp_4 {
    top: -128px
}

.note_d_4,
.note_dsharp_4 {
    top: -136px
}

.note_e_4 {
    top: -144px
}

.note_f_4,
.note_fsharp_4 {
    top: -152px
}

.note_g_4,
.note_gsharp_4 {
    top: -160px
}

.note_a_4,
.note_asharp_4 {
    top: -168px
}

.note_b_4 {
    top: -176px
}
</style>

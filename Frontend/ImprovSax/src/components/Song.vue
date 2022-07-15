<template>
  <div class="songWrapper">
    <div class="staff">
      <div></div>
      <div></div>
      <div></div>
      <div></div>
      <div></div>
    </div>
    <div id="notesWrapper">
      <div
        class="noteHolder"
        v-if="notes?.length > 0"
        v-for="(note, i) in notes"
        v-bind:key="i"
        :style="[{ top: `${note.top}px` }]"
        :class="[{ show: note.show }]"
      >
        <p
          class="note"
          :id="`${note.value}_${i}`"
          :class="[{ strikethrough: note.soloOctave == 4 }, { isSharp: note.isSharp }]"
          :style="[{ backgroundColor: note.backgroundColor }, notes.expansion]"
        ></p>
        <span>&#9834;</span>
      </div>
    </div>
    <div class="start-btn">
      <button v-on:click="play_all">Start</button>
    </div>
  </div>
</template>
<script async setup lang="ts">
import { reactive, ref } from "vue";
import { Api } from "../api/api";
const props = defineProps({
  songName: { type: String, required: true },
});

const play_all = () => {
  var audio = new Audio("a_train.mp3");
  audio.play();
  setTimeout(() => play(notes, 0), 5000);
};
const timeout = async (callback: () => void, ms: number) => {
  return new Promise((resolve) => setTimeout(resolve, ms)).then((x) => callback());
};
const startAnimating = (note: any, index: number) => {
  const element: HTMLElement | null = document.getElementById(`${note.value}_${index}`);
  element?.animate(
    [
      {
        transform: `scale(${note.duration.doubleDuration / 100})`,
      },
    ],
    {
      duration: note.duration.doubleDuration,
      iterations: 1,
      fill: "forwards",
    },
  );
};
const play = async (notes: any, index: number) => {
  const currentNote = notes[index];
  console.log(`playing ${currentNote.value} for index ${index}`);
  currentNote.show = true;
  const audio = new Audio(`chrom_sax/${currentNote.value}.mp3`);
  audio.volume = 0.2;
  await audio.play();
  startAnimating(currentNote, index);
  timeout(() => {
    play(notes, ++index);
    timeout(() => audio.pause(), 50);
  }, currentNote.duration.doubleDuration - 50);
};
const calculateBackground = () => {
  return `rgba(
            ${Math.floor(Math.random() * 255)},
            ${Math.floor(Math.random() * 255)},
            ${Math.floor(Math.random() * 255)},
          0.3)`;
};
const calculatePosition = (noteValue: string, noteOctave: string) => {
  const letterOptions = ["a", "b", "c", "d", "e", "f", "g"];
  const noteBaseline = letterOptions.indexOf(noteValue[0]) * -11;
  const octaveBaseline = (parseInt(noteOctave) - 1) * -99;
  const position = octaveBaseline - noteBaseline;
  console.log(`${noteValue}_${noteOctave}: ${position}px`);
  return position;
};
const calculateIsSharp = (noteValue: string) => {
  return noteValue.includes("sharp");
};
const getNotes = async () => {
  const notes = await api.getNotes(new Date(), props.songName);
  const mappedNotes = notes.map((x: any) => {
    return {
      ...x,
      isSharp: calculateIsSharp(x.soloValue),
      top: calculatePosition(x.soloValue, x.soloOctave),
      backgroundColor: calculateBackground(),
      show: false,
      play: false,
      ready: false,
    };
  });
  return mappedNotes;
};

const api = new Api();
const tempNotes = await getNotes();
const notes = reactive(tempNotes);
</script>

<style lang="scss">
.start-btn {
  position: relative;

  top: 100px;
}
.songWrapper,
.staff {
  position: absolute;
  width: 100%;
  top: 50%;
  left: 0;
}

#notesWrapper {
  position: relative;
  top: 54px;
}

body {
  padding-top: 50px;
  padding-bottom: 20px;
}

.body-content {
  padding-left: 15px;
  padding-right: 15px;
}

input,
select,
textarea {
  max-width: 280px;
}

.staff {
  margin-top: -50px;
  height: 100px;
}

.staff > div {
  width: 100%;
  height: 2px;
  background: #000;
  margin-top: 20px;
}

.noteHolder {
  display: none;
  left: 50%;
  position: absolute;
  &.show {
    display: block;
    animation: move 6s linear;
    animation-fill-mode: forwards;
  }
  .note {
    position: relative;
    width: 100px;
    height: 100px;
    border-radius: 50%;
    background: rgba(72, 0, 200, 0.3);

    & + span {
      font-size: 70px;
      display: flex;
      align-items: center;
      justify-content: center;
      position: relative;
      top: -107px;
    }
  }
}

.isSharp span::before {
  content: "♯";
  font-size: 28px;
  position: absolute;
  left: 50%;
  top: 50%;
  margin-left: -30px;
  margin-top: -20px;
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
  margin-left: -23px;
}

@keyframes move {
  0% {
    left: 50%;
  }

  100% {
    left: -100%;
  }
}
</style>

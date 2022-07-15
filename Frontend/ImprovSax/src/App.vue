<template>
  <div>
    <template v-if="songName === null">
      <p>Pick a song</p>
      <select v-model="songName" name="songPicker" id="songPicker">
        <option v-for="option in songOptions" :value="option.file">{{ option.name }}</option>
      </select>
    </template>
    <template v-else>
      <Suspense>
        <Song :songName="songName" />
      </Suspense>
    </template>
  </div>
</template>

<script setup lang="ts">
import { ref } from "vue";
import Song from "./components/Song.vue";
const songName: any = ref("a_train");
let songOptions: any = [{ name: "Take the A Train", file: "a_train" }];
</script>

<style></style>

<script setup>
import { useDataStore } from '../stores/data.js'
import { onBeforeMount } from 'vue'
const dataStore = useDataStore()

// Ensure loadDataFromApi is an asynchronous action
const loadData = async () => {
  await dataStore.loadDataFromApi()
  // Additional logic if needed after the API call
}

// Call loadData when the component is created
onBeforeMount(loadData)
</script>
<style></style>
<template>
  <h2>This is the data page where data is pulled from api</h2>
  <hr />
    <table class="table-auto">
        <thead>
        <tr>
            <th class="px-4 py-2">Date</th>
            <th class="px-4 py-2">Temp (C)</th>
            <th class="px-4 py-2">Temp (F)</th>
            <th class="px-4 py-2">Summary</th>
        </tr>
        </thead>
        <tbody>
        <tr v-for="item in dataStore.data" :key="item">
            <td class="border px-4 py-2">{{ item.date }}</td>
            <td class="border px-4 py-2">{{ item.temperatureC }}</td>
            <td class="border px-4 py-2">{{ item.temperatureF }}</td>
            <td class="border px-4 py-2">{{ item.summary }}</td>
        </tr>
        </tbody>
    </table>
</template>

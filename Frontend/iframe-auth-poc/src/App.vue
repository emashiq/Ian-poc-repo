<script setup>
import { RouterLink, RouterView } from 'vue-router'
import { useMsalStore } from './stores/microsoft-auth.js'
import { PublicClientApplication } from '@azure/msal-browser'
import { onMounted } from 'vue'
import router from './router'
const msalConfig = useMsalStore()
const msalInstance = new PublicClientApplication(msalConfig)
const request = {
  scopes: ['openid', 'profile', 'email', 'api://d797761a-7fe4-4bbc-a324-8feb56cbdd7d/user_app_api']
}

onMounted(async () => {
  console.log(this)
  await msalInstance.initialize()
      msalInstance
        .handleRedirectPromise()
        .then(async (res) => {
          var accessToken = res.accessToken

          var formData = new FormData()
          formData.append('code', accessToken)
          formData.append('email', res.account.username)
          formData.append('localAccountId', res.account.localAccountId)

          fetch('/LoginByCode', {
            method: 'POST',
            body: formData
          })
            .then((response) => {
              if (!response.ok) {
                throw new Error(`HTTP error! Status: ${response.status}`)
              }
              router.push('/about')
              return response.text() // or response.json() if the server returns JSON
            })
            .then((data) => {
              console.log(data)
            })
            .catch((error) => {
              console.error('Fetch error:', error)
            })
        })
        .catch((err) => {
          console.error(err)
        })
})

const login = async () => {
  await msalInstance.initialize()
  await msalInstance.loginRedirect(request)
}

const logout = async () => {
  await msalInstance.initialize()
  //fetch logout
  let logoutResult = await fetch('/Logout')
  if (logoutResult.ok) {
    msalInstance.logoutRedirect()
  } else {
    console.log('Logout failed')
  }
}
</script>

<template>
  <nav class="bg-white border-gray-200 dark:bg-gray-900">
    <div class="max-w-screen-xl flex flex-wrap items-center justify-between mx-auto p-4">
      <a href="https://flowbite.com/" class="flex items-center space-x-3 rtl:space-x-reverse">
        <img src="https://flowbite.com/docs/images/logo.svg" class="h-8" alt="Flowbite Logo" />
        <span class="self-center text-2xl font-semibold whitespace-nowrap dark:text-white"
          >POC Iframe WebForm</span
        >
      </a>
      <button
        data-collapse-toggle="navbar-default"
        type="button"
        class="inline-flex items-center p-2 w-10 h-10 justify-center text-sm text-gray-500 rounded-lg md:hidden hover:bg-gray-100 focus:outline-none focus:ring-2 focus:ring-gray-200 dark:text-gray-400 dark:hover:bg-gray-700 dark:focus:ring-gray-600"
        aria-controls="navbar-default"
        aria-expanded="false"
      >
        <span class="sr-only">Open main menu</span>
        <svg
          class="w-5 h-5"
          aria-hidden="true"
          xmlns="http://www.w3.org/2000/svg"
          fill="none"
          viewBox="0 0 17 14"
        >
          <path
            stroke="currentColor"
            stroke-linecap="round"
            stroke-linejoin="round"
            stroke-width="2"
            d="M1 1h15M1 7h15M1 13h15"
          />
        </svg>
      </button>
      <div class="hidden w-full md:block md:w-auto" id="navbar-default">
        <ul
          class="font-medium flex flex-col p-4 md:p-0 mt-4 border border-gray-100 rounded-lg bg-gray-50 md:flex-row md:space-x-8 rtl:space-x-reverse md:mt-0 md:border-0 md:bg-white dark:bg-gray-800 md:dark:bg-gray-900 dark:border-gray-700"
        >
          <li>
            <RouterLink
              to="/"
              class="block py-2 px-3 text-white bg-blue-700 rounded md:bg-transparent md:text-blue-700 md:p-0 dark:text-white md:dark:text-blue-500"
              aria-current="page"
              >Home</RouterLink
            >
          </li>
          <li>
            <RouterLink
              to="/data"
              class="block py-2 px-3 text-white bg-blue-700 rounded md:bg-transparent md:text-blue-700 md:p-0 dark:text-white md:dark:text-blue-500"
              aria-current="page"
              >Data</RouterLink
            >
          </li>
          <li>
            <RouterLink
              to="/about"
              class="block py-2 px-3 text-gray-900 rounded hover:bg-gray-100 md:hover:bg-transparent md:border-0 md:hover:text-blue-700 md:p-0 dark:text-white md:dark:hover:text-blue-500 dark:hover:bg-gray-700 dark:hover:text-white md:dark:hover:bg-transparent"
              >About</RouterLink
            >
          </li>
          <li>
            <button
              @click="logout"
              class="block py-2 px-3 text-gray-900 rounded hover:bg-gray-100 md:hover:bg-transparent md:border-0 md:hover:text-blue-700 md:p-0 dark:text-white md:dark:hover:text-blue-500 dark:hover:bg-gray-700 dark:hover:text-white md:dark:hover:bg-transparent"
            >
              Logout
            </button>
          </li>
          <li>
            <button
              @click="login"
              class="block py-2 px-3 text-gray-900 rounded hover:bg-gray-100 md:hover:bg-transparent md:border-0 md:hover:text-blue-700 md:p-0 dark:text-white md:dark:hover:text-blue-500 dark:hover:bg-gray-700 dark:hover:text-white md:dark:hover:bg-transparent"
            >
              Login
            </button>
          </li>
        </ul>
      </div>
    </div>
  </nav>
  <div class="md:container md:mx-auto">
    <RouterView />
  </div>
</template>

<style scoped></style>

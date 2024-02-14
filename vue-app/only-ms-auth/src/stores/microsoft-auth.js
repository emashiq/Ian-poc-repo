import { ref } from 'vue'
import { defineStore } from 'pinia'

export const useMsalStore = defineStore('msalStore', () => {
  const auth = ref({
    auth: {
      clientId: 'd797761a-7fe4-4bbc-a324-8feb56cbdd7d',
      authority: 'https://login.microsoftonline.com/e663c283-8efd-43d1-8459-16d6f2066761',
      redirectUri: window.location.origin,
      postLogoutRedirectUri: window.location.origin,
      navigateToLoginRequestUrl: true
    },
    cache: {
      cacheLocation: 'localStorage'
    },
    system: {
      allowNativeBroker: false
    },
    
  })
  const authenticated = ref(false)


  const setIsAuthenticated = (isAuthenticated) => {
    authenticated.value = isAuthenticated
  }

  return { auth,authenticated: authenticated.value, setIsAuthenticated }
})

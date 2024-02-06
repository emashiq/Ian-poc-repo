//create simple data store using pinia
import { defineStore } from 'pinia'
import { useMsalStore } from './microsoft-auth'
import { PublicClientApplication } from '@azure/msal-browser'
const apiBasePath = 'https://localhost:7093/'

const msalConfig = useMsalStore()

const publicClientApplication = new PublicClientApplication(msalConfig)

export const useDataStore = defineStore('dataStore', {
  state: () => ({
    data: []
  }),
  actions: {
    loadDataFromApi() {
      publicClientApplication.initialize().then(() => {
        const account = publicClientApplication.getAllAccounts()[0]

        const accessTokenRequest = {
          scopes: ['api://d797761a-7fe4-4bbc-a324-8feb56cbdd7d/user_app_api'],
          account: account
        }
        publicClientApplication.acquireTokenSilent(accessTokenRequest).then((response) => {
          fetch(apiBasePath + 'WeatherForecast', {
            headers: {
              Authorization: 'Bearer ' + response.accessToken
            }
          })
            .then((response) => {
              return response.json()
            })
            .then((items) => {
              this.data = items
            })
        })
      })
    }
  }
})

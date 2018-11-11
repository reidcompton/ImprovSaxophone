const datalayer = {
    serviceUrl: 'https://improvsax20181110122842.azurewebsites.net/api/home',
    get () {
      const response = fetch(this.serviceUrl)
      return response
    },
}  

export default datalayer

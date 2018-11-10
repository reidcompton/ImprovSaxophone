const datalayer = {
    serviceUrl: 'https://localhost:44330/api/home',
    get (start, song) {
      const response = fetch(this.serviceUrl)
      return response
    },
}  

export default datalayer

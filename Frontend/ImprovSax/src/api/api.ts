export class Api {
  baseUrl: String = "https://localhost:7209/api/home";

  async getNotes(start: Date, songName: String) {
    const response = await fetch(`${this.baseUrl}?start=${start.toISOString()}&songName=${songName}`).then((response) =>
      response.json(),
    );
    return response;
  }
}

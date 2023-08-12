import { Image } from "@/Models/Image";
import { Trip } from "@/Models/Trip";
import axios from 'axios'

class BaseApiService {
    baseUrl = "api";
    resource;
  
    constructor(resource : string) {
      if (!resource) throw new Error("Resource is not provided");
      this.resource = resource;
    }
  
    getUrl(id: string | number = "") {
      return `${this.baseUrl}/${this.resource}/${id}`;
    }
  
    handleErrors(err: string) {
      // Note: here you may want to add your errors handling
      console.log({ message: "Error in API call: ", err });
    }
  }

  class ImagesApiService extends BaseApiService {
    constructor() {
        super("images");
    }

    async fetch(year : number | null = null) : Promise<Image[] | null> {
      // Set query parameters
      const params: any = {};
      if (year != null) {
        params.year = year;
      }

      return axios.get<Image[]>(super.getUrl(), {params: params})
        .then(response => response.data)
        .catch(err => {
          this.handleErrors(err);
          return null;
        })
    }

    async get(id : number ) : Promise<Image | null> {
      return axios.get<Image>(super.getUrl(id))
        .then(response => response.data)
        .catch(err => {
          this.handleErrors(err);
          return null;
        })
    }

    async upload(image: File, year: number) : Promise<Image | null> {
      const form = new FormData()
      form.append('year', year.toString())
      form.append('file', image)
      return fetch(super.getUrl(), {
        method: 'POST',
        body: form
        }
      )
      .then(response => {
        if (!response.ok) {
          this.handleErrors(response.statusText);
          return null;
        }
        else {
          return response.json() as Promise<Image>
        }
      })
    }
  }

  class TripsApiService extends BaseApiService {
    constructor() {
        super("trips");
    }
    
    async fetch() : Promise<Trip[] | null> {
      return axios.get<Trip[]>(super.getUrl())
        .then(response => response.data)
        .catch(err => {
          this.handleErrors(err);
          return null;
        })
    }

    async get(year : number ) : Promise<Trip | null> {
      return axios.get<Trip>(super.getUrl(year))
        .then(response => response.data)
        .catch(err => {
          this.handleErrors(err);
          return null;
        })
    }
  }

  class TokenService extends BaseApiService {
    constructor() {
        super("token");
    }

    async get() : Promise<string | null> {
      return axios.get<string>(super.getUrl())
        .then(response => response.data)
        .catch(err => {
          this.handleErrors(err);
          return null;
        })
    }
  }

  export const $api = {
    images: new ImagesApiService(),
    trips: new TripsApiService(),
    token: new TokenService()
  }
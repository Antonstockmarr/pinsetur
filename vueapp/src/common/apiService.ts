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

    async fetch(year : number | null = null, onlyCovers : boolean | null = null) : Promise<Image[] | null> {
      // Set query parameters
      const params: any = {};
      if (year != null) {
        params.year = year;
      }
      if (onlyCovers != null) {
        params.onlyCovers = onlyCovers;
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

    async download(id : number) : Promise<string | null> {
      return axios({
        method: 'get',
        url: super.getUrl(id) + "/download",
        responseType: "blob"
      })
        .then(response => URL.createObjectURL(response.data))
        .catch(err => {
          this.handleErrors(err);
          return null;
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

  export const $api = {
    images: new ImagesApiService(),
    trips: new TripsApiService()
  }
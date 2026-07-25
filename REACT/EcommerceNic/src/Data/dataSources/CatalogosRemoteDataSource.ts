import { Platform } from "react-native";

const API_URL =
    Platform.OS === "android"
        ? "http://10.0.2.2:5092"
        : "http://127.0.0.1:5092";

export interface PaisResponse {
    id: number;
    nombre: string;
}

export interface GeneroResponse {
    id: number;
    nombre: string;
}

class CatalogosRemoteDataSource {

    async getPaises(): Promise<PaisResponse[]> {

        const response = await fetch(`${API_URL}/api/Catalogos_/Paises`);

        if (!response.ok) {
            throw new Error("No se pudieron obtener los países.");
        }

        return await response.json();
    }

    async getGeneros(): Promise<GeneroResponse[]> {

        const response = await fetch(`${API_URL}/api/Catalogos_/Generos`);

        if (!response.ok) {
            throw new Error("No se pudieron obtener los géneros.");
        }

        return await response.json();
    }

}

export const catalogosRemoteDataSource =
    new CatalogosRemoteDataSource();
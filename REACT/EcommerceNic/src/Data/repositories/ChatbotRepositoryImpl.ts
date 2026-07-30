import { ChatbotRepository } from "../../Domain/repositories/ChatbotRepository";
import { chatbotRemoteDataSource } from "../dataSources/ChatbotRemoteDataSource";
import { ChatbotResponse } from "../dataSources/ChatbotRemoteDataSource";


export class ChatbotRepositoryImpl implements ChatbotRepository {


    async sendMessage(
        message: string,
        conversacionId?: number
    ): Promise<ChatbotResponse> {
        return chatbotRemoteDataSource.sendMessage(message, conversacionId);
    }

}
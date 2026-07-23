import { ChatbotResponse } from "../../Data/dataSources/ChatbotRemoteDataSource";

export interface ChatbotRepository {

    sendMessage(
        message:string
    ): Promise<ChatbotResponse>;

}
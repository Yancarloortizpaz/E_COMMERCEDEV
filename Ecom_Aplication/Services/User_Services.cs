using Ecom_Domain;
using modu.application.Interface;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ecom_Aplication.Services
{
    public class User_Services : IUserRepository
    {
        private readonly IUserRepository _userRepository;

        public User_Services(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<IEnumerable<User>> LISTAR_USER_ASYNC()
        {
            return await _userRepository.LISTAR_USER_ASYNC();
        }

        public async Task<IEnumerable<User>> FILTRAR_USER_ASYNC(string searchTerm)
        {
            return await _userRepository.FILTRAR_USER_ASYNC(searchTerm);
        }

        public async Task<(int code, string message, int? templateId)> NUEVO_USER_ASYNC(
            string userFullName,
            string userName,
            string userPasswordPlain,
            string userEmail,
            string userPhoneNumber,
            int userCountryId,
            int userGenderId,
            DateTime userBirthDay,
            int userCreatorId,
            int userStatusId)
        {
            return await _userRepository.NUEVO_USER_ASYNC(
                userFullName,
                userName,
                userPasswordPlain,
                userEmail,
                userPhoneNumber,
                userCountryId,
                userGenderId,
                userBirthDay,
                userCreatorId,
                userStatusId
            );
        }

        public async Task<(int code, string message, int? templateId)> ACTUALIZAR_USER_ASYNC(
            int userId,
            string userFullName,
            string userEmail,
            string userPhoneNumber,
            int userModificatorId,
            int userStatusId,
            bool forzarRecuperacion)
        {
            return await _userRepository.ACTUALIZAR_USER_ASYNC(
                userId,
                userFullName,
                userEmail,
                userPhoneNumber,
                userModificatorId,
                userStatusId,
                forzarRecuperacion
            );
        }

        public async Task<(int code, string message)> ELIMINAR_USER_ASYNC(int userId, int userModificatorId)
        {
            return await _userRepository.ELIMINAR_USER_ASYNC(userId, userModificatorId);
        }
    }
}
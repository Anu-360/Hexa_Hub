/* eslint-disable no-unused-vars */
import { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import ToastNotification, { showToast } from '../Utils/ToastNotification';
import axios from 'axios';
import { jwtDecode } from 'jwt-decode';
import Cookies from 'js-cookie';

const SignInPage = () => {
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const [error, setError] = useState('');
  const navigate = useNavigate();
  const [showPassword, setShowPassword] = useState(false);

  const handleLogoClick = (e) => {
    e.preventDefault();
    navigate('/');
  };

  const handleLogin = async (e) => {
    e.preventDefault();
    const loginData = {
      UserMail: email,
      Password: password,
    };

    try {
      const response = await axios.post('https://localhost:7287/api/Auth', loginData);

      const { token } = response.data;
      Cookies.set('token', token);
      // Decoding JWT token for fetching User Role and navigating to the appropriate dashboard
      const decode = jwtDecode(token);
      console.log(decode);

      const userRole = decode.role;
      setTimeout(() => {
        if (userRole === 'Admin') {
          navigate('/admin/Dashboard');
        } else if(userRole === 'Employee') {
          // navigate('/dashboard');
          navigate('/dashboard');
        }
        else{
          alert('error');
        }
      }, 2000);

      showToast('Login Successful!', 'success')

    } catch (err) {
      const errorMessage = err.response?.data?.message || 'Invalid credentials';
      console.error('Error during login:', err);

      showToast('Failed to Log In. Please try again.', 'error');
      setError(errorMessage);
    }
  };

  return (
    <div className="min-h-screen bg-gray-100 flex flex-col justify-center px-4 sm:px-6">
      <div className="absolute top-0 left-0 p-4 sm:p-6 md:p-8 hidden lg:contents mt-6 sm:mt-12">
        <div className="flex items-center mt-0 bg-transparent">
          <img
            alt="HexaHub Logo"
            src="../Images/logo.png"
            className="h-10 w-auto cursor-pointer bg-transparent"
            onClick={handleLogoClick}
          />
          <p
            className="text-xl font-semibold text-gray-900 ml-2 cursor-pointer"
            onClick={handleLogoClick}
          >
            HexaHub
          </p>
        </div>
      </div>

      <button
        onClick={() => navigate('/')}
        className="absolute top-4 right-4 text-gray-500 hover:text-gray-700"
      >
        ✖
      </button>

      <div className="w-full max-w-md mx-auto">
        <h2 className="mt-6 text-center text-3xl font-extrabold text-gray-900">
          Sign in to HexaHub
        </h2>

        <div className="mt-8 bg-white py-8 px-4 shadow sm:rounded-lg sm:px-10">
          <form className="space-y-4" onSubmit={handleLogin}>
            <input
              className="text-black bg-white w-full px-3 py-2 border border-gray-300 rounded-md"
              type="email"
              placeholder="Email"
              value={email}
              onChange={(e) => setEmail(e.target.value)}
            />
            <div className="relative">
              <input
                className="text-black bg-white w-full px-3 py-2 border border-gray-300 rounded-md"
                type={showPassword ? 'text' : 'password'}
                placeholder="Password"
                value={password}
                onChange={(e) => setPassword(e.target.value)}
              />
              <button
                type="button"
                className="bg-transparent absolute inset-y-0 right-0 pr-3 flex items-center focus:outline-none"
                onClick={() => setShowPassword(!showPassword)}
              >
                {showPassword ? '👁️' : '👁️‍🗨️'}
              </button>
            </div>
            <div>
              <button
                type="submit"
                className="w-full flex justify-center py-2 px-4 border border-transparent rounded-md shadow-sm text-sm font-medium text-white bg-[#F87060] hover:bg-[#f85842] focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-[#F87060]"
              >
                Sign In
              </button>
            </div>
          </form>
          <ToastNotification />
          <div className="mt-6 flex items-center justify-between text-sm">
            <a href="#" className="font-medium text-indigo-600 hover:text-indigo-500">Privacy</a>
            <a href="#" className="font-medium text-indigo-600 hover:text-indigo-500">Terms</a>
          </div>
        </div>
      </div>
    </div>
  );
};

export default SignInPage;

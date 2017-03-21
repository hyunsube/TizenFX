/*
 * Copyright (c) 2016 Samsung Electronics Co., Ltd All Rights Reserved
 *
 * Licensed under the Apache License, Version 2.0 (the License);
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 * http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an AS IS BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

namespace Tizen.Multimedia
{
    /// <summary>
    /// Specifies the supported QR code encoding mode.
    /// </summary>
    public enum QrMode
    {
        /// <summary>
        /// Numeric digits.
        /// </summary>
        Numeric,
        /// <summary>
        /// Alphanumeric characters, '$', '%', '*', '+', '-', '.', '/' and ':'.
        /// </summary>
        AlphaNumeric,
        /// <summary>
        /// Raw 8-bit bytes.
        /// </summary>
        Byte,
        /// <summary>
        /// UTF-8 character encoding.
        /// </summary>
        Utf8
    }
}

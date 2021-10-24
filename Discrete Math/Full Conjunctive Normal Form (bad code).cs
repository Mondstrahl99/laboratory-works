using System;
using System.Collections;

namespace SKNF
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Введите булевую функцию:\n");
            string text = Console.ReadLine();
            string[] stringSeparators = new string[] { "->", "v", "&", "<=>", "-", "(", ")" };
            string[] letters = text.Split(stringSeparators, StringSplitOptions.RemoveEmptyEntries);
            ArrayList list_of_letters = new ArrayList();
            for (int i = 0; i < letters.Length; i++)
            {
                if (!list_of_letters.Contains(letters[i])) list_of_letters.Add(letters[i]);
            }
            list_of_letters.Sort();
            int row = list_of_letters.Count;
            int col = Convert.ToInt32(Math.Pow(2, row));
            int[,] matrix = new int[row, col];
            int k = 2;
            for (int i = 0; i < row; ++i)
            {
                int filler = 0;
                int score = 0;
                for (int j = 0; j < col; ++j)
                {
                    if (score < (col + 1) / k)
                    {
                        matrix[i, j] = filler;
                        score++;
                    }
                    else
                    {
                        score = 0;
                        filler = (filler == 0) ? 1 : 0;
                        matrix[i, j] = filler;
                        score++;
                    }
                }
                k *= 2;
            }
            k /= 2;
            ArrayList list_of_results = new ArrayList();
            int insert_index = -k;
            int rm_bracket_i = 0;
            int nothing;
            bool rm_bracket_switch = false;
            int operation_checker_amp = 1;
            int operation_checker_v = 1;
            bool main_checker = true;
            bool extra_operation_checker = true;
            bool extra_operation_checker2 = true;
            bool minus_ending = false;
            bool imp_ending = false;
            bool eq_ending = false;

            string[] stringSeparators_for_brackets = new string[] { "+" };
            string[] stringSeparators_for_brackets_question_mark = new string[] { "(?", "?)" };
            text = text.Replace("(", "+(+").Replace(")", "+)+");
            string[] without_brackets = text.Split(stringSeparators_for_brackets, StringSplitOptions.RemoveEmptyEntries);

            while ((without_brackets.Length > 1 || (without_brackets.Length == 1 && without_brackets[0].Contains('-'))) || main_checker)
            {
                string txt = "";
                for (int text_i = 0; text_i < without_brackets.Length; ++text_i)
                {
                    int last_sign_amp = without_brackets[text_i].LastIndexOf("&");
                    int last_sign_v = without_brackets[text_i].LastIndexOf("v");
                    if (last_sign_amp == without_brackets[text_i].Length - 1) extra_operation_checker = false;
                    if (last_sign_v == without_brackets[text_i].Length - 1) extra_operation_checker = false;
                    int first_sign_amp = without_brackets[text_i].IndexOf("&");
                    int first_sign_v = without_brackets[text_i].IndexOf("v");
                    if (first_sign_amp == 0) extra_operation_checker2 = false;
                    if (first_sign_v == 0) extra_operation_checker2 = false;
                    bool length_check = without_brackets[text_i].Length >= 3;
                    if (without_brackets[text_i].Length == 2 && without_brackets[text_i].Contains('-')) length_check = true;
                    if (without_brackets[text_i].Length - 1 == without_brackets[text_i].LastIndexOf('-')) minus_ending = true;
                    if (without_brackets[text_i].Length - 1 == without_brackets[text_i].LastIndexOf('>')) imp_ending = true;
                    else if (without_brackets[text_i].IndexOf("->") == 0) imp_ending = true;
                    if (without_brackets[text_i].Length - 1 == without_brackets[text_i].LastIndexOf('>')) eq_ending = true;
                    else if (without_brackets[text_i].IndexOf("<=>") == 0) eq_ending = true;

                    if (length_check && extra_operation_checker && extra_operation_checker2 && !minus_ending && !imp_ending && !eq_ending)
                    {
                        text = without_brackets[text_i];
                        //minus
                        for(;;)
                        {
                            int[] array = new int[col];
                            if (!text.Contains('-')) break;
                            int sign_min = text.IndexOf('-');
                            if (text.Contains("->-"))
                            {
                                sign_min = text.IndexOf("->-") + 2;
                            }
                            else if (text[sign_min + 1] == '>' && (text.IndexOf('-') == text.LastIndexOf('-'))) break;
                            for(int i = 0; i < text.Length; ++i)
                            {
                                if (text[i] == '-' && text[i + 1] != '>') sign_min = i;
                            }
                            if (Char.IsLetter(text[sign_min + 1]))
                            {
                                for (int n = 0; n < col; ++n)
                                {
                                    array[n] = (matrix[(text[sign_min + 1] - 'a'), n] == 0) ? 1 : 0;
                                }
                                list_of_results.AddRange(array);
                                text = text.Insert(sign_min + 2, Convert.ToString(insert_index += k));
                                text = text.Remove(sign_min, 2);
                            }
                            else if(Char.IsNumber(text[sign_min +1]))
                            {
                                int num = 0;
                                int t = 1;
                                bool border = true;
                                int R = 0;                                                     
                                for (int n = 0; n < k; ++n)
                                {
                                    if (border)
                                    {
                                        while (Char.IsNumber(text, sign_min + t))
                                        {
                                            R++;
                                            num = num * 10 + (Convert.ToInt32(text[sign_min + t]) - '0');
                                            t++;
                                            if (sign_min + t == text.Length)
                                            {
                                                border = false;
                                                break;
                                            }
                                        }
                                    }
                                    array[n] = (Convert.ToInt32(list_of_results[num + n]) == 0) ? 1 : 0;
                                }
                                list_of_results.AddRange(array);
                                text = text.Insert(sign_min + R + 1, Convert.ToString(insert_index += k));
                                text = text.Remove(sign_min, R + 1);
                            }
                        }                        
                        //&
                        for (;;)
                        {
                            int sign_amp = text.IndexOf("&");
                            bool sign_amp_checker;
                            if (sign_amp == -1) sign_amp_checker = false;
                            else sign_amp_checker = true;
                            //letters with &
                            if (!sign_amp_checker) break;
                            if (Char.IsLetter(text[sign_amp - 1])  && Char.IsLetter(text[sign_amp + 1]))
                            {
                                int[] array = new int[col];
                                for (int j = 0; j < col; ++j)
                                {
                                    array[j] = matrix[(text[sign_amp - 1] - 'a'), j] & matrix[(text[sign_amp + 1] - 'a'), j];
                                }
                                list_of_results.AddRange(array);
                                text = text.Insert(sign_amp + 2, Convert.ToString(insert_index += k));
                                text = text.Remove(sign_amp - 1, 3);
                                if (!text.Contains('&')) break;                                
                            }
                            //numbers with &  
                            sign_amp = text.IndexOf("&");
                            if (sign_amp == -1) sign_amp_checker = false;
                            if (!sign_amp_checker) break;
                            if (Char.IsNumber(text[sign_amp - 1]) && Char.IsNumber(text[sign_amp + 1]))
                            {                                
                                int[] array = new int[col];
                                int[] arr1 = new int[col];
                                int[] arr2 = new int[col];
                                int num;
                                int t;
                                bool border;
                                int R;
                                int L;                               
                                for (int j = 0; j < col; ++j)
                                {
                                    border = true;
                                    t = 0;
                                    L = 0;
                                    num = -1;
                                    int k_10 = 1;
                                    for (int n = 0; n < k; ++n)
                                    {                                        
                                        if(border)
                                        {
                                            while (Char.IsNumber(text, sign_amp - t - 1))
                                            {
                                                L++;
                                                if (num == -1) num = Convert.ToInt32(text[sign_amp - t - 1]) - '0';
                                                else num = num + (Convert.ToInt32(text[sign_amp - t - 1]) - '0') * (k_10 *= 10);
                                                t++;
                                                if (sign_amp - t - 1 < 0)
                                                {
                                                    border = false;
                                                    break;
                                                }
                                            }
                                        }                                        
                                        arr1[n] = Convert.ToInt32(list_of_results[num + n]);
                                    }
                                    t = 0;
                                    R = 0;
                                    border = true;
                                    num = 0;
                                    for (int n = 0; n < k; ++n)
                                    {
                                        if (border)
                                        {
                                            while (Char.IsNumber(text, sign_amp + t + 1))
                                            {
                                                R++;
                                                num = num * 10 + (Convert.ToInt32(text[sign_amp + t + 1]) - '0');
                                                t++;
                                                if (sign_amp + t + 1 == text.Length)
                                                {
                                                    border = false;
                                                    break;
                                                }
                                            }
                                        }
                                        arr2[n] = Convert.ToInt32(list_of_results[num + n]);
                                    }
                                    for (int n = 0; n < col; ++n)
                                    {
                                        array[n] = arr1[n] & arr2[n];
                                    }
                                    list_of_results.AddRange(array);                                                      
                                    text = text.Insert(R + sign_amp + 1, Convert.ToString(insert_index += k));
                                    text = text.Remove(sign_amp - L, L + R + 1);
                                    if (!text.Contains('&')) break;
                                    sign_amp = text.IndexOf("&");
                                    if (!(Char.IsNumber(text[sign_amp - 1]) && Char.IsNumber(text[sign_amp + 1]))) break;
                                }
                            }
                            // left - number, right - letter with &
                            sign_amp = text.IndexOf("&");
                            if (sign_amp == -1) sign_amp_checker = false;
                            if (!sign_amp_checker) break;
                            if (Char.IsNumber(text[sign_amp - 1]) && Char.IsLetter(text[sign_amp + 1]))
                            {                               
                                int[] array = new int[col];
                                int[] arr1 = new int[col];
                                int[] arr2 = new int[col];
                                int num;
                                int t;
                                bool border;
                                int L;                               
                                for (int j = 0; j < col; ++j)
                                {
                                    //number
                                    border = true;
                                    t = 0;
                                    L = 0;
                                    num = -1;
                                    int k_10 = 1;
                                    for (int n = 0; n < k; ++n)
                                    {
                                        if (border)
                                        {
                                            while (Char.IsNumber(text, sign_amp - t - 1))
                                            {
                                                L++;
                                                if (num == -1) num = Convert.ToInt32(text[sign_amp - t - 1]) - '0';
                                                else num = num + (Convert.ToInt32(text[sign_amp - t - 1]) - '0') * (k_10 *= 10);
                                                t++;
                                                if (sign_amp - t - 1 < 0)
                                                {
                                                    border = false;
                                                    break;
                                                }
                                            }
                                        }
                                        arr1[n] = Convert.ToInt32(list_of_results[num + n]);                                            
                                    }
                                    //letter
                                    for (int n = 0; n < col; ++n)
                                    {
                                        arr2[n] = matrix[(text[sign_amp + 1] - 'a'), n];
                                    }
                                    for (int n = 0; n < col; ++n)
                                    {
                                        array[n] = arr1[n] & arr2[n];
                                    }
                                    list_of_results.AddRange(array);                                    
                                    text = text.Insert(sign_amp + 2, Convert.ToString(insert_index += k));
                                    text = text.Remove(sign_amp - L, L + 2);                                    
                                    if (!text.Contains('&')) break;
                                    sign_amp = text.IndexOf("&");
                                    if (!(Char.IsNumber(text[sign_amp - 1]) && Char.IsLetter(text[sign_amp + 1]))) break;
                                }
                            }
                            // left - letter, right - number with &
                            sign_amp = text.IndexOf("&");
                            if (sign_amp == -1) sign_amp_checker = false;
                            if (!sign_amp_checker) break;
                            if (Char.IsLetter(text[sign_amp - 1]) && Char.IsNumber(text[sign_amp + 1]))
                            {                               
                                int[] array = new int[col];
                                int[] arr1 = new int[col];
                                int[] arr2 = new int[col];
                                int num;
                                int t;
                                bool border;
                                int R;
                                for (int j = 0; j < col; ++j)
                                {
                                    //letter
                                    for (int n = 0; n < col; ++n)
                                    {
                                        arr1[n] = matrix[(text[sign_amp - 1] - 'a'), n];
                                    }
                                    //number
                                    border = true;
                                    t = 0;
                                    R = 0;
                                    num = 0;
                                    for (int n = 0; n < k; ++n)
                                    {
                                        if (border)
                                        {
                                            while (Char.IsNumber(text, sign_amp + t + 1))
                                            {
                                                R++;
                                                num = num * 10 + (Convert.ToInt32(text[sign_amp + t + 1]) - '0');
                                                t++;
                                                if (sign_amp + t + 1 == text.Length)
                                                {
                                                    border = false;
                                                    break;
                                                }
                                            }
                                        }
                                        arr2[n] = Convert.ToInt32(list_of_results[num + n]);
                                    }
                                    for (int n = 0; n < col; ++n)
                                    {
                                        array[n] = arr1[n] & arr2[n];
                                    }
                                    list_of_results.AddRange(array);                                   
                                    text = text.Insert(sign_amp + R + 1, Convert.ToString(insert_index += k));
                                    text = text.Remove(sign_amp - 1, R + 2);                                    
                                    if (!text.Contains('&')) break;
                                    sign_amp = text.IndexOf("&");
                                    if (!(Char.IsLetter(text[sign_amp - 1]) && Char.IsNumber(text[sign_amp + 1]))) break;
                                }
                            }                            
                        }
                        //v
                        for (;;)
                        {
                            int sign_v = text.IndexOf("v");
                            bool sign_v_checker;
                            if (sign_v == -1) sign_v_checker = false;
                            else sign_v_checker = true;
                            //letters with v
                            if (!sign_v_checker) break;
                            if (Char.IsLetter(text[sign_v - 1]) && Char.IsLetter(text[sign_v + 1]))
                            {
                                int[] array = new int[col];
                                for (int j = 0; j < col; ++j)
                                {
                                    array[j] = matrix[(text[sign_v - 1] - 'a'), j] | matrix[(text[sign_v + 1] - 'a'), j];
                                }
                                list_of_results.AddRange(array);
                                text = text.Insert(sign_v + 2, Convert.ToString(insert_index += k));
                                text = text.Remove(sign_v - 1, 3);
                                if (!text.Contains('v')) break;                                
                            }
                            //numbers with v  
                            sign_v = text.IndexOf("v");
                            if (sign_v == -1) sign_v_checker = false;
                            if (!sign_v_checker) break;
                            if (Char.IsNumber(text[sign_v - 1]) && Char.IsNumber(text[sign_v + 1]))
                            {
                                int[] array = new int[col];
                                int[] arr1 = new int[col];
                                int[] arr2 = new int[col];
                                int num;
                                int t;
                                bool border;
                                int R;
                                int L;                                
                                for (int j = 0; j < col; ++j)
                                {
                                    border = true;
                                    t = 0;
                                    L = 0;
                                    num = -1;
                                    int k_10 = 1;
                                    for (int n = 0; n < k; ++n)
                                    {
                                        if (border)
                                        {
                                            while (Char.IsNumber(text, sign_v - t - 1))
                                            {
                                                L++;
                                                if (num == -1) num = Convert.ToInt32(text[sign_v - t - 1]) - '0';
                                                else num = num + (Convert.ToInt32(text[sign_v - t - 1]) - '0') * (k_10 *= 10);
                                                t++;
                                                if (sign_v - t - 1 < 0)
                                                {
                                                    border = false;
                                                    break;
                                                }
                                            }
                                        }
                                        arr1[n] = Convert.ToInt32(list_of_results[num + n]);
                                    }
                                    t = 0;
                                    R = 0;
                                    border = true;
                                    num = 0;                                    
                                    for (int n = 0; n < k; ++n)
                                    {
                                        if (border)
                                        {
                                            while (Char.IsNumber(text, sign_v + t + 1))
                                            {
                                                R++;
                                                num = num * 10 + (Convert.ToInt32(text[sign_v + t + 1]) - '0');
                                                t++;
                                                if (sign_v + t + 1 == text.Length)
                                                {
                                                    border = false;
                                                    break;
                                                }
                                            }
                                        }
                                        arr2[n] = Convert.ToInt32(list_of_results[num + n]);
                                    }
                                    for (int n = 0; n < col; ++n)
                                    {
                                        array[n] = arr1[n] | arr2[n];
                                    }
                                    list_of_results.AddRange(array);
                                    text = text.Insert(R + sign_v + 1, Convert.ToString(insert_index += k));
                                    text = text.Remove(sign_v - L, L + R + 1);
                                    if (!text.Contains('v')) break;
                                    sign_v = text.IndexOf("v");
                                    if (!(Char.IsNumber(text[sign_v - 1]) && Char.IsNumber(text[sign_v + 1]))) break;
                                }
                            }
                            // left - number, right - letter with v
                            sign_v = text.IndexOf("v");
                            if (sign_v == -1) sign_v_checker = false;
                            if (!sign_v_checker) break;
                            if (Char.IsNumber(text[sign_v - 1]) && Char.IsLetter(text[sign_v + 1]))
                            {
                                int[] array = new int[col];
                                int[] arr1 = new int[col];
                                int[] arr2 = new int[col];
                                int num;
                                int t;
                                bool border;
                                int L;                                
                                for (int j = 0; j < col; ++j)
                                {
                                    //number
                                    border = true;
                                    t = 0;
                                    L = 0;
                                    num = -1;
                                    int k_10 = 1;
                                    for (int n = 0; n < k; ++n)
                                    {
                                        if (border)
                                        {
                                            while (Char.IsNumber(text, sign_v - t - 1))
                                            {
                                                L++;
                                                if (num == -1) num = Convert.ToInt32(text[sign_v - t - 1]) - '0';
                                                else num = num + (Convert.ToInt32(text[sign_v - t - 1]) - '0') * (k_10 *= 10);
                                                t++;
                                                if (sign_v - t - 1 < 0)
                                                {
                                                    border = false;
                                                    break;
                                                }
                                            }
                                        }
                                        arr1[n] = Convert.ToInt32(list_of_results[num + n]);
                                    }
                                    //letter
                                    for (int n = 0; n < col; ++n)
                                    {
                                        arr2[n] = matrix[(text[sign_v + 1] - 'a'), n];
                                    }
                                    for (int n = 0; n < col; ++n)
                                    {
                                        array[n] = arr1[n] | arr2[n];
                                    }
                                    list_of_results.AddRange(array);
                                    text = text.Insert(sign_v + 2, Convert.ToString(insert_index += k));
                                    text = text.Remove(sign_v - L, L + 2);
                                    if (!text.Contains('v')) break;
                                    sign_v = text.IndexOf("v");
                                    if (!(Char.IsNumber(text[sign_v - 1]) && Char.IsLetter(text[sign_v + 1]))) break;
                                }
                            }
                            // left - letter, right - number with v
                            sign_v = text.IndexOf("v");
                            if (sign_v == -1) sign_v_checker = false;
                            if (!sign_v_checker) break;
                            if (Char.IsLetter(text[sign_v - 1]) && Char.IsNumber(text[sign_v + 1]))
                            {
                                int[] array = new int[col];
                                int[] arr1 = new int[col];
                                int[] arr2 = new int[col];
                                int num;
                                int t;
                                bool border;
                                int R;
                                for (int j = 0; j < col; ++j)
                                {
                                    //letter
                                    for (int n = 0; n < col; ++n)
                                    {
                                        arr1[n] = matrix[(text[sign_v - 1] - 'a'), n];
                                    }
                                    //number
                                    border = true;
                                    t = 0;
                                    R = 0;
                                    num = 0;
                                    for (int n = 0; n < k; ++n)
                                    {
                                        if (border)
                                        {
                                            while (Char.IsNumber(text, sign_v + t + 1))
                                            {
                                                R++;
                                                num = num * 10 + (Convert.ToInt32(text[sign_v + t + 1]) - '0');
                                                t++;
                                                if (sign_v + t + 1 == text.Length)
                                                {
                                                    border = false;
                                                    break;
                                                }
                                            }
                                        }
                                        arr2[n] = Convert.ToInt32(list_of_results[num + n]);
                                    }
                                    for (int n = 0; n < col; ++n)
                                    {
                                        array[n] = arr1[n] | arr2[n];
                                    }
                                    list_of_results.AddRange(array);
                                    text = text.Insert(sign_v + R + 1, Convert.ToString(insert_index += k));
                                    text = text.Remove(sign_v - 1, R + 2);
                                    if (!text.Contains('v')) break;
                                    sign_v = text.IndexOf("v");
                                    if (!(Char.IsLetter(text[sign_v - 1]) && Char.IsNumber(text[sign_v + 1]))) break;
                                }
                            }
                        }
                        //->
                        for (;;)
                        {
                            int sign_imp = text.IndexOf("->");
                            bool sign_imp_checker;
                            if (sign_imp == -1) sign_imp_checker = false;
                            else sign_imp_checker = true;
                            //letters with ->
                            if (!sign_imp_checker) break;
                            if (Char.IsLetter(text[sign_imp - 1]) && Char.IsLetter(text[sign_imp + 2]))
                            {
                                int[] array = new int[col];
                                for (int j = 0; j < col; ++j)
                                {
                                    array[j] = (matrix[(text[sign_imp - 1] - 'a'), j] <= matrix[(text[sign_imp + 2] - 'a'), j]) ? 1 : 0;
                                }
                                list_of_results.AddRange(array);
                                text = text.Insert(sign_imp + 3, Convert.ToString(insert_index += k));
                                text = text.Remove(sign_imp - 1, 4);
                                if (!text.Contains("->")) break;
                            }
                            //numbers with ->  
                            sign_imp = text.IndexOf("->");
                            if (sign_imp == -1) sign_imp_checker = false;
                            if (!sign_imp_checker) break;
                            if (Char.IsNumber(text[sign_imp - 1]) && Char.IsNumber(text[sign_imp + 2]))
                            {
                                int[] array = new int[col];
                                int[] arr1 = new int[col];
                                int[] arr2 = new int[col];
                                int num;
                                int t;
                                bool border;
                                int R;
                                int L;
                                for (int j = 0; j < col; ++j)
                                {
                                    border = true;
                                    t = 0;
                                    L = 0;
                                    num = -1;
                                    int k_10 = 1;
                                    for (int n = 0; n < k; ++n)
                                    {
                                        if (border)
                                        {
                                            while (Char.IsNumber(text, sign_imp - t - 1))
                                            {
                                                L++;
                                                if (num == -1) num = Convert.ToInt32(text[sign_imp - t - 1]) - '0';
                                                else num = num + (Convert.ToInt32(text[sign_imp - t - 1]) - '0') * (k_10 *= 10);
                                                t++;
                                                if (sign_imp - t - 1 < 0)
                                                {
                                                    border = false;
                                                    break;
                                                }
                                            }
                                        }
                                        arr1[n] = Convert.ToInt32(list_of_results[num + n]);
                                    }
                                    t = 0;
                                    R = 0;
                                    border = true;
                                    num = 0;
                                    for (int n = 0; n < k; ++n)
                                    {
                                        if (border)
                                        {
                                            while (Char.IsNumber(text, sign_imp + t + 2))
                                            {
                                                R++;
                                                num = num * 10 + (Convert.ToInt32(text[sign_imp + t + 2]) - '0');
                                                t++;
                                                if (sign_imp + t + 2 == text.Length)
                                                {
                                                    border = false;
                                                    break;
                                                }
                                            }
                                        }
                                        arr2[n] = Convert.ToInt32(list_of_results[num + n]);
                                    }
                                    for (int n = 0; n < col; ++n)
                                    {
                                        array[n] = (arr1[n] <= arr2[n]) ? 1 : 0;
                                    }
                                    list_of_results.AddRange(array);
                                    text = text.Insert(R + sign_imp + 2, Convert.ToString(insert_index += k));
                                    text = text.Remove(sign_imp - L, L + R + 2);
                                    if (!text.Contains("->")) break;
                                    sign_imp = text.IndexOf("->");
                                    if (!(Char.IsNumber(text[sign_imp - 1]) && Char.IsNumber(text[sign_imp + 2]))) break;
                                }
                            }
                            // left - number, right - letter with ->
                            sign_imp = text.IndexOf("->");
                            if (sign_imp == -1) sign_imp_checker = false;
                            if (!sign_imp_checker) break;
                            if (Char.IsNumber(text[sign_imp - 1]) && Char.IsLetter(text[sign_imp + 2]))
                            {
                                int[] array = new int[col];
                                int[] arr1 = new int[col];
                                int[] arr2 = new int[col];
                                int num;
                                int t;
                                bool border;
                                int L;
                                for (int j = 0; j < col; ++j)
                                {
                                    //number
                                    border = true;
                                    t = 0;
                                    L = 0;
                                    num = -1;
                                    int k_10 = 1;
                                    for (int n = 0; n < k; ++n)
                                    {
                                        if (border)
                                        {
                                            while (Char.IsNumber(text, sign_imp - t - 1))
                                            {
                                                L++;
                                                if (num == -1) num = Convert.ToInt32(text[sign_imp - t - 1]) - '0';
                                                else num = num + (Convert.ToInt32(text[sign_imp - t - 1]) - '0') * (k_10 *= 10);
                                                t++;
                                                if (sign_imp - t - 1 < 0)
                                                {
                                                    border = false;
                                                    break;
                                                }
                                            }
                                        }
                                        arr1[n] = Convert.ToInt32(list_of_results[num + n]);
                                    }
                                    //letter
                                    for (int n = 0; n < col; ++n)
                                    {
                                        arr2[n] = matrix[(text[sign_imp + 2] - 'a'), n];
                                    }
                                    for (int n = 0; n < col; ++n)
                                    {
                                        array[n] = (arr1[n] <= arr2[n]) ? 1 : 0;
                                    }
                                    list_of_results.AddRange(array);
                                    text = text.Insert(sign_imp + 3, Convert.ToString(insert_index += k));
                                    text = text.Remove(sign_imp - L, L + 3);
                                    if (!text.Contains("->")) break;
                                    sign_imp = text.IndexOf("->");
                                    if (!(Char.IsNumber(text[sign_imp - 1]) && Char.IsLetter(text[sign_imp + 2]))) break;
                                }
                            }
                            // left - letter, right - number with ->
                            sign_imp = text.IndexOf("->");
                            if (sign_imp == -1) sign_imp_checker = false;
                            if (!sign_imp_checker) break;
                            if (Char.IsLetter(text[sign_imp - 1]) && Char.IsNumber(text[sign_imp + 2]))
                            {
                                int[] array = new int[col];
                                int[] arr1 = new int[col];
                                int[] arr2 = new int[col];
                                int num;
                                int t;
                                bool border;
                                int R;
                                for (int j = 0; j < col; ++j)
                                {
                                    //letter
                                    for (int n = 0; n < col; ++n)
                                    {
                                        arr1[n] = matrix[(text[sign_imp - 1] - 'a'), n];
                                    }
                                    //number
                                    border = true;
                                    t = 0;
                                    R = 0;
                                    num = 0;
                                    for (int n = 0; n < k; ++n)
                                    {
                                        if (border)
                                        {
                                            while (Char.IsNumber(text, sign_imp + t + 2))
                                            {
                                                R++;
                                                num = num * 10 + (Convert.ToInt32(text[sign_imp + t + 2]) - '0');
                                                t++;
                                                if (sign_imp + t + 2 == text.Length)
                                                {
                                                    border = false;
                                                    break;
                                                }
                                            }
                                        }
                                        arr2[n] = Convert.ToInt32(list_of_results[num + n]);
                                    }
                                    for (int n = 0; n < col; ++n)
                                    {
                                        array[n] = (arr1[n] <= arr2[n]) ? 1 : 0;
                                    }
                                    list_of_results.AddRange(array);
                                    text = text.Insert(sign_imp + R + 2, Convert.ToString(insert_index += k));
                                    text = text.Remove(sign_imp - 1, R + 3);
                                    if (!text.Contains("->")) break;
                                    sign_imp = text.IndexOf("->");
                                    if (!(Char.IsLetter(text[sign_imp - 1]) && Char.IsNumber(text[sign_imp + 2]))) break;
                                }
                            }
                        }
                        //<=>
                        for (;;)
                        {
                            int sign_eq = text.IndexOf("<=>");
                            bool sign_eq_checker;
                            if (sign_eq == -1) sign_eq_checker = false;
                            else sign_eq_checker = true;
                            //letters with <=>
                            if (!sign_eq_checker) break;
                            if (Char.IsLetter(text[sign_eq - 1]) && Char.IsLetter(text[sign_eq + 3]))
                            {
                                int[] array = new int[col];
                                for (int j = 0; j < col; ++j)
                                {
                                    array[j] = (matrix[(text[sign_eq - 1] - 'a'), j] == matrix[(text[sign_eq + 3] - 'a'), j]) ? 1 : 0;
                                }
                                list_of_results.AddRange(array);
                                text = text.Insert(sign_eq + 4, Convert.ToString(insert_index += k));
                                text = text.Remove(sign_eq - 1, 5);
                                if (!text.Contains("<=>")) break;
                            }
                            //numbers with <=>  
                            sign_eq = text.IndexOf("<=>");
                            if (sign_eq == -1) sign_eq_checker = false;
                            if (!sign_eq_checker) break;
                            if (Char.IsNumber(text[sign_eq - 1]) && Char.IsNumber(text[sign_eq + 3]))
                            {
                                int[] array = new int[col];
                                int[] arr1 = new int[col];
                                int[] arr2 = new int[col];
                                int num;
                                int t;
                                bool border;
                                int R;
                                int L;
                                for (int j = 0; j < col; ++j)
                                {
                                    border = true;
                                    t = 0;
                                    L = 0;
                                    num = -1;
                                    int k_10 = 1;
                                    for (int n = 0; n < k; ++n)
                                    {
                                        if (border)
                                        {
                                            while (Char.IsNumber(text, sign_eq - t - 1))
                                            {
                                                L++;
                                                if (num == -1) num = Convert.ToInt32(text[sign_eq - t - 1]) - '0';
                                                else num = num + (Convert.ToInt32(text[sign_eq - t - 1]) - '0') * (k_10 *= 10);
                                                t++;
                                                if (sign_eq - t - 1 < 0)
                                                {
                                                    border = false;
                                                    break;
                                                }
                                            }
                                        }
                                        arr1[n] = Convert.ToInt32(list_of_results[num + n]);
                                    }
                                    t = 0;
                                    R = 0;
                                    border = true;
                                    num = 0;
                                    for (int n = 0; n < k; ++n)
                                    {
                                        if (border)
                                        {
                                            while (Char.IsNumber(text, sign_eq + t + 3))
                                            {
                                                R++;
                                                num = num * 10 + (Convert.ToInt32(text[sign_eq + t + 3]) - '0');
                                                t++;
                                                if (sign_eq + t + 3 == text.Length)
                                                {
                                                    border = false;
                                                    break;
                                                }
                                            }
                                        }
                                        arr2[n] = Convert.ToInt32(list_of_results[num + n]);
                                    }
                                    for (int n = 0; n < col; ++n)
                                    {
                                        array[n] = (arr1[n] == arr2[n]) ? 1 : 0;
                                    }
                                    list_of_results.AddRange(array);
                                    text = text.Insert(R + sign_eq + 3, Convert.ToString(insert_index += k));
                                    text = text.Remove(sign_eq - L, L + R + 3);
                                    if (!text.Contains("<=>")) break;
                                    sign_eq = text.IndexOf("<=>");
                                    if (!(Char.IsNumber(text[sign_eq - 1]) && Char.IsNumber(text[sign_eq + 3]))) break;
                                }
                            }
                            // left - number, right - letter with <=>
                            sign_eq = text.IndexOf("<=>");
                            if (sign_eq == -1) sign_eq_checker = false;
                            if (!sign_eq_checker) break;
                            if (Char.IsNumber(text[sign_eq - 1]) && Char.IsLetter(text[sign_eq + 3]))
                            {
                                int[] array = new int[col];
                                int[] arr1 = new int[col];
                                int[] arr2 = new int[col];
                                int num;
                                int t;
                                bool border;
                                int L;
                                for (int j = 0; j < col; ++j)
                                {
                                    //number
                                    border = true;
                                    t = 0;
                                    L = 0;
                                    num = -1;
                                    int k_10 = 1;
                                    for (int n = 0; n < k; ++n)
                                    {
                                        if (border)
                                        {
                                            while (Char.IsNumber(text, sign_eq - t - 1))
                                            {
                                                L++;
                                                if (num == -1) num = Convert.ToInt32(text[sign_eq - t - 1]) - '0';
                                                else num = num + (Convert.ToInt32(text[sign_eq - t - 1]) - '0') * (k_10 *= 10);
                                                t++;
                                                if (sign_eq - t - 1 < 0)
                                                {
                                                    border = false;
                                                    break;
                                                }
                                            }
                                        }
                                        arr1[n] = Convert.ToInt32(list_of_results[num + n]);
                                    }
                                    //letter
                                    for (int n = 0; n < col; ++n)
                                    {
                                        arr2[n] = matrix[(text[sign_eq + 3] - 'a'), n];
                                    }
                                    for (int n = 0; n < col; ++n)
                                    {
                                        array[n] = (arr1[n] == arr2[n]) ? 1 : 0;
                                    }
                                    list_of_results.AddRange(array);
                                    text = text.Insert(sign_eq + 4, Convert.ToString(insert_index += k));
                                    text = text.Remove(sign_eq - L, L + 4);
                                    if (!text.Contains("<=>")) break;
                                    sign_eq = text.IndexOf("<=>");
                                    if (!(Char.IsNumber(text[sign_eq - 1]) && Char.IsLetter(text[sign_eq + 3]))) break;
                                }
                            }
                            // left - letter, right - number with <=>
                            sign_eq = text.IndexOf("<=>");
                            if (sign_eq == -1) sign_eq_checker = false;
                            if (!sign_eq_checker) break;
                            if (Char.IsLetter(text[sign_eq - 1]) && Char.IsNumber(text[sign_eq + 3]))
                            {
                                int[] array = new int[col];
                                int[] arr1 = new int[col];
                                int[] arr2 = new int[col];
                                int num;
                                int t;
                                bool border;
                                int R;
                                for (int j = 0; j < col; ++j)
                                {
                                    //letter
                                    for (int n = 0; n < col; ++n)
                                    {
                                        arr1[n] = matrix[(text[sign_eq - 1] - 'a'), n];
                                    }
                                    //number
                                    border = true;
                                    t = 0;
                                    R = 0;
                                    num = 0;
                                    for (int n = 0; n < k; ++n)
                                    {
                                        if (border)
                                        {
                                            while (Char.IsNumber(text, sign_eq + t + 3))
                                            {
                                                R++;
                                                num = num * 10 + (Convert.ToInt32(text[sign_eq + t + 3]) - '0');
                                                t++;
                                                if (sign_eq + t + 3 == text.Length)
                                                {
                                                    border = false;
                                                    break;
                                                }
                                            }
                                        }
                                        arr2[n] = Convert.ToInt32(list_of_results[num + n]);
                                    }
                                    for (int n = 0; n < col; ++n)
                                    {
                                        array[n] = (arr1[n] == arr2[n]) ? 1 : 0;
                                    }
                                    list_of_results.AddRange(array);
                                    text = text.Insert(sign_eq + R + 3, Convert.ToString(insert_index += k));
                                    text = text.Remove(sign_eq - 1, R + 4);
                                    if (!text.Contains("<=>")) break;
                                    sign_eq = text.IndexOf("<=>");
                                    if (!(Char.IsLetter(text[sign_eq - 1]) && Char.IsNumber(text[sign_eq + 3]))) break;
                                }
                            }
                        }

                        if (txt.Length == 0) txt = text;
                        else txt += text;
                        rm_bracket_i = txt.Length - 1;
                        rm_bracket_switch = true;
                    }
                    else
                    {
                        if (txt.Length == 0) txt = without_brackets[text_i];
                        else txt += without_brackets[text_i];
                        if (rm_bracket_switch)
                        {
                            int number = Convert.ToInt32(text);
                            int s = -1;
                            if (number != 0)
                            {
                                for (; number > 0; s++)
                                {
                                    number = number / 10;
                                }
                            }
                            else s = 0;
                            txt = txt.Insert(rm_bracket_i - s, "?");
                            txt = txt.Insert(rm_bracket_i + 2, "?");
                            rm_bracket_switch = false;
                        }
                        extra_operation_checker = true;
                        extra_operation_checker2 = true;
                        minus_ending = false;
                        imp_ending = false;
                        eq_ending = false;
                    }
                }
                without_brackets = txt.Split(stringSeparators_for_brackets_question_mark, StringSplitOptions.RemoveEmptyEntries);
                txt = " ";
                for (int txt_i = 0; txt_i < without_brackets.Length; ++txt_i)
                {
                    txt += without_brackets[txt_i];
                }
                txt = txt.Trim();
                txt = txt.Replace("(", "+(+").Replace(")", "+)+");
                without_brackets = txt.Split(stringSeparators_for_brackets, StringSplitOptions.RemoveEmptyEntries);
                operation_checker_amp = without_brackets[0].IndexOf("&");
                operation_checker_v = without_brackets[0].IndexOf("v");
                operation_checker_amp++;
                operation_checker_v++;
                main_checker = true;
                if (operation_checker_amp + operation_checker_v == 0) main_checker = false;
                if (Int32.TryParse(text, out nothing) && without_brackets.Length == 1) main_checker = true;
            }
            bool remove = false;
            string result = null;
            for(int i = 0; i < col; ++i)
            {
                if(!remove) result += '(';
                remove = false;
                for (int j = 0; j < row; ++j)
                {                   
                    if(Convert.ToInt32(list_of_results[list_of_results.Count - k + i]) == 0)
                    {
                        if (matrix[j, i] == 0) result += Convert.ToChar(j + 'a');
                        else
                        {
                            result += '-';
                            result += Convert.ToChar(j + 'a');
                        }
                        if(j != row - 1) result += 'v';
                    }
                    else remove = true;
                }               
                if (!remove)
                {
                   result += ')';
                   result += '&';
                }               
            }
            result = result.TrimEnd('(');
            result = result.Trim('&');
            Console.WriteLine("\nСКНФ:");
            Console.WriteLine(result);
            Console.WriteLine();
        }
    }
}

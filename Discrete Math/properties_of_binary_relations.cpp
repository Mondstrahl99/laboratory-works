#include <iostream> 

#include <iomanip>


void fill_matrix(const char s[], int** matrix);
int size_of_matrix(const char s[]);
void print_matrix(int size, int** matrix);
void print_set(int size);
void reflexive_irreflexive(int size, int** matrix);
void anti_reflexive_non_anti_reflexive(int size, int** matrix);
void symmetric_asymmetric(int size, int** matrix);
void anti_symmetric_non_anti_asymmetric(int size, int** matrix);
void transitive_intransitive(int size, int** matrix);

int main()
{
    const int MAX = 500;
    char string[MAX];
    std::cout << "Enter binary relations" << std::endl;
    fgets(string, MAX, stdin);
    int size = size_of_matrix(string);
    print_set(size);
    std::cout << "Relations matrix with size " << size << "x" << size 
              << std::endl; 
    int **matrix = new int*[size];
    for (int i = 0; i < size; ++i){
        matrix[i] = new int[size];
    }
    fill_matrix(string, matrix);
    print_matrix(size, matrix); 
    std::cout << "Properties of binary relation" << std::endl;
    reflexive_irreflexive(size, matrix);
    anti_reflexive_non_anti_reflexive(size, matrix);
    symmetric_asymmetric(size, matrix);
    anti_symmetric_non_anti_asymmetric(size, matrix);
    transitive_intransitive(size, matrix);
    std::cout << std::endl;
    std::cout << "Performed by Sagirov A.I. group 4109" << std::endl;
    for (int i = 0; i < size; ++i)
        delete [] matrix[i];
    delete [] matrix;
    return 0;
}

int size_of_matrix(const char s[])
{
    int size = 0, number1 = 0, number2 = 0;
    for (int i = 0; s[i] != '\0'; ++i){
        if((s[i] >= '0') && (s[i] <= '9')){
            for (;s[i] != ','; ++i) {
                number1 = (number1 * 10) + (s[i] - '0');
            }
            ++i;
            for (;(s[i] != '\0') && (s[i] != ')'); ++i){              
                number2 = (number2 * 10) + (s[i] - '0');
            }            
        }  
        if((number1 > number2) && (number1 > size)) size = number1;
        else if((number2 > number1) && (number2 > size)) size = number2;
        else if(number1 > size) size = number1;
        number1 = 0;
        number2 = 0;
    }
    return size;
}

void fill_matrix(const char s[], int** matrix)
{
    int number1 = 0, number2 = 0;
    for (int i = 0; s[i] != '\0'; ++i){
        if((s[i] >= '0') && (s[i] <= '9')){
            for (;s[i] != ','; ++i) {
                number1 = (number1 * 10) + (s[i] - '0');
            }
            ++i;
            for (;(s[i] != '\0') && (s[i] != ')'); ++i){              
                number2 = (number2 * 10) + (s[i] - '0');
            }
            matrix[--number1][--number2] = 1;              
            number1 = 0;
            number2 = 0;
        }  
    }
} 

void print_matrix(int size, int** matrix)
{
    for(int i = 0; i < size; ++i){
        for(int j = 0; j < size; ++j)
            std::cout << "   " << matrix[i][j];
        std::cout << std::endl;
    }
}

void print_set(int size)
{
    std::cout << "The set on wich the relation is given" << std::endl;
    std::cout << "A = {";
    for(int i = 1; i < size; ++i)
        std::cout << i << ", ";
    std::cout << size << "}" << std::endl;
}

void reflexive_irreflexive(int size, int** matrix)
{
    bool reflexive = true;
    for(int i = 0; i < size; ++i)
        if(matrix[i][i] == 0) reflexive = false;
    if(reflexive) std::cout << "-Reflexive" << std::endl;
    else std::cout << "-Irreflexive" << std::endl;
}

void anti_reflexive_non_anti_reflexive(int size, int** matrix)
{
    bool anti_reflexive = true;
    for(int i = 0; i < size; ++i)
        if(matrix[i][i] == 1) anti_reflexive = false;
    if(anti_reflexive) std::cout << "-Anti-reflexive" << std::endl;
    else std::cout << "-Non-anti-reflexive" << std::endl;
}

void symmetric_asymmetric(int size, int** matrix) 
{
    bool symmetric = true;
    for(int i = 0; i < size; ++i)
        for(int j = 0; j < size; ++j)
            if(i != j)
               if(matrix[i][j] != matrix[j][i]) symmetric = false;
    if(symmetric) std::cout << "-Symmetric" << std::endl;
    else std::cout << "-Asymmetric" << std::endl;
}

void anti_symmetric_non_anti_asymmetric(int size, int** matrix)
{
    bool anti_symmetric = true;
    for(int i = 0; i < size; ++i)
        for(int j = 0; j < size; ++j)
            if(i != j)
               if(matrix[i][j] == matrix[j][i]) anti_symmetric = false;
    if(anti_symmetric) std::cout << "-Anti-symmetric" << std::endl;
    else std::cout << "-Non-anti-symmetric" << std::endl;
}

void transitive_intransitive(int size, int** matrix)
{
    bool intransitive = true, element = true;
    for(int i = 0; i < size; ++i){
        for(int j = 0; j < size; ++j){
            element = false;
            for(int k = 0; k < size; ++k)
                element = (matrix[i][k] && matrix[k][j]) || element;
            intransitive = element && !matrix[i][j]; 
            if(intransitive) break;
        }
        if(intransitive) break;
    }   
    if(intransitive) std::cout << "-Intransitive" << std::endl;
    else std::cout << "-Transitive" << std::endl;
}


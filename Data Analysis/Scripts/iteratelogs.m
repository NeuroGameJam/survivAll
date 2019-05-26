function [] = iteratelogs(Nfiles)

itvec = 6:Nfiles;

for i = itvec
    path = strcat('Log_',num2str(i));
    ReadLog(path);
end
end
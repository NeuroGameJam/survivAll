function [] = iteratelogs(Nfiles)

itvec = horzcat(0:2, 4:Nfiles);

for i = itvec
    path = strcat('Log_',num2str(i));
    ReadLog(path);
end
end
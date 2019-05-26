function [log, worldvec] = ReadLog(path)

logfile = fopen(path, 'r');
celllog = {};

for i = 1:2
    tline = fgets(logfile);
end

while ischar(tline)
    tline = fgets(logfile);
    celllog = [celllog; tline];
end
clear i

fclose(logfile);

Nmoves = size(celllog, 1)-1;
log = cell(Nmoves, 7);

for i = 1:Nmoves
    entry = strsplit(celllog{i});
    log(i,:) = entry;
end
clear i

turnvec = log(:,1);
movesvec = log(:,2);
wealthvec = log(:,3);
worldvec = log(:,4);
deltavec = log(:,5);
tilevec = log(:,6);

clear log;
log = zeros(Nmoves, 5);

for j = 1:1:Nmoves
    a = worldvec{j};
    log(j,1:5) = [str2num(turnvec{j}), str2num(wealthvec{j}), str2double(worldvec{j}), ...
                  str2double(deltavec{j}), str2num(tilevec{j})];
end
clear j

clear turnvec wealthvec worldvec deltavec

highestbin = max(log(:,5))+0.5;
markeroffset = 0.3*max(log(:,3));

f = figure
subplot 121
hold all
plot(log(:,1), log(:,2), 'r-')
plot(log(:,1), log(:,3), 'g-')
plot(log(:,1), log(:,4), 'y-')
text(log(:,1), repmat(markeroffset, Nmoves, 1), movesvec, 'FontSize', 14)
text(log(:,1), repmat(1.5*markeroffset, Nmoves, 1), tilevec, 'Fontsize', 10)
text(4, 0.9*markeroffset, 'Action', 'Fontsize', 10)
text(4, 1.6*markeroffset, 'Current Tile', 'Fontsize', 8)
legend('Wealth', 'World', 'Delta')
xlabel('Moves')
xlim([0 Nmoves+1])

subplot 122
histogram(log(:,5), 0.5:1:highestbin)
xlabel('Tile')
ylabel('Number of visits')
title('Tile Occupancy')
xlim([0.5; highestbin])

saveas(f, strcat(path,'.jpg'))
end
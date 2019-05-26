function [log, worldvec] = ReadLog(path)

logfile = fopen(path, 'r');
celllog = {};

tline = strsplit(fgets(logfile), ';');
Ntiles = str2num(cell2mat(tline(4)));

tline = fgets(logfile);


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

markeroffset = 0.3*max(log(:,3));
log(:,5) = log(:,5)+1;

figure

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
title('Game Dynamics')

subplot 122
PlotHeatMap(log(:, end),Ntiles);

% saveas(f, strcat(path,'.jpg'))
end